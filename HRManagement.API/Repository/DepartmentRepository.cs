using HRManagement.API.Data;
using HRManagement.API.DTOs.Departments;
using HRManagement.API.Exceptions;
using HRManagement.API.Interfaces;
using HRManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.API.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HRContext _context;

        public DepartmentRepository(HRContext context)
        {
            _context = context;
        }

        private IQueryable<Department> BaseQuery() =>
            _context.Departments
                .Include(d => d.Location)
                .AsNoTracking();
        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await BaseQuery()
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(decimal departmentId)
        {
            return await BaseQuery()
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);
        }

        public async Task<IEnumerable<Department>> GetByLocationAsync(decimal locationId)
        {
            return await BaseQuery()
                .Where(d => d.LocationId == locationId)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetByManagerAsync(decimal managerId)
        {
            return await BaseQuery()
                .Where(d => d.ManagerId == managerId)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Department>> SearchByNameAsync(string name)
        {
            return await BaseQuery()
                .Where(d => d.DepartmentName.Contains(name))
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Department> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, int pageSize)
        {
            var query = BaseQuery().OrderBy(d => d.DepartmentName);

            int totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<DepartmentSummaryDto> GetSummaryAsync()
        {
            var departments = await _context.Departments.AsNoTracking().ToListAsync();

            int total = departments.Count;
            int withManager = departments.Count(d => d.ManagerId.HasValue);
            int withLocation = departments.Count(d => d.LocationId.HasValue);

            double avgEmployees = 0;
            if (total > 0)
            {
                int totalEmployees = await _context.Employees.CountAsync();
                avgEmployees = Math.Round((double)totalEmployees / total, 2);
            }

            return new DepartmentSummaryDto
            {
                TotalDepartments = total,
                DepartmentsWithManager = withManager,
                DepartmentsWithoutManager = total - withManager,
                DepartmentsWithLocation = withLocation,
                DepartmentsWithoutLocation = total - withLocation,
                AverageEmployeesPerDepartment = avgEmployees
            };
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Departments.CountAsync();
        }
        public async Task<IEnumerable<Department>> GetDropdownAsync()
        {
            return await _context.Departments
                .AsNoTracking()
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(decimal departmentId)
        {
            return await _context.Departments.AnyAsync(d => d.DepartmentId == departmentId);
        }

        public async Task<bool> NameExistsAsync(string name, decimal? excludeId = null)
        {
            return await _context.Departments.AnyAsync(d =>
                d.DepartmentName == name &&
                (excludeId == null || d.DepartmentId != excludeId));
        }

        public async Task<Department> CreateAsync(Department department)
        {
            int nextId = await _context.Departments.AnyAsync()
                ? (int)(await _context.Departments.MaxAsync(d => (decimal)d.DepartmentId)) + 10
                : 10;

            if (nextId > 9999)
                throw new ValidationException("DepartmentId limit is completed.");

            department.DepartmentId = nextId;

            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<Department> UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
            return department;
        }
    }
}
