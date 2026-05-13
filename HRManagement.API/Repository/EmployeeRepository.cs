using HRManagement.API.Data;
using HRManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.API.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRContext _context;

<<<<<<< Updated upstream
        public EmployeeRepository( HRContext context)
=======
        public EmployeeRepository(HRContext context)
>>>>>>> Stashed changes
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Job)
                .Include(e => e.Department)
                .Include(e => e.Manager)
                .ToListAsync();
        }

<<<<<<< Updated upstream
        public async Task<Employee?> GetByIdAsync(decimal id)
=======
        public async Task<Employee?> GetByIdAsync(int id)
>>>>>>> Stashed changes
        {
            return await _context.Employees
                .Include(e => e.Job)
                .Include(e => e.Department)
                .Include(e => e.Manager)
<<<<<<< Updated upstream
                .FirstOrDefaultAsync(e =>e.EmployeeId == id);
        }


        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e =>e.Email == email);
        }


        public async Task<List<Employee>> GetManagersAsync()
        {
            return await _context.Employees
                .Where(e =>   _context.Employees .Any(x => x.ManagerId == e.EmployeeId))
                .ToListAsync();
        }


        public async Task<List<Employee>>  GetSubordinatesAsync(decimal managerId)
        {
            return await _context.Employees .Where(e => e.ManagerId == managerId)
=======
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<List<Employee>> GetManagersAsync()
        {
            return await _context.Employees
                .Where(e => _context.Employees.Any(x => x.ManagerId == e.EmployeeId))
                .ToListAsync();
        }

        public async Task<List<Employee>> GetSubordinatesAsync(int managerId)
        {
            return await _context.Employees
                .Where(e => e.ManagerId == managerId)
>>>>>>> Stashed changes
                .Include(e => e.Job)
                .ToListAsync();
        }

<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
        public async Task<List<Employee>> SearchEmployeesAsync(string name)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Job)
                .Where(e => (e.FirstName + " " + e.LastName)
                    .ToLower()
                    .Contains(name.ToLower()))
                .ToListAsync();
        }

<<<<<<< Updated upstream

        public async Task<List<Employee>> GetEmployeesByDepartmentAsync(short departmentId)
        {
            return await _context.Employees
                .Where(e =>e.DepartmentId == departmentId)
=======
        public async Task<List<Employee>> GetEmployeesByDepartmentAsync(short departmentId)
        {
            return await _context.Employees
                .Where(e => e.DepartmentId == departmentId)
>>>>>>> Stashed changes
                .Include(e => e.Job)
                .ToListAsync();
        }

<<<<<<< Updated upstream

        public async Task<bool> EmployeeExistsAsync(decimal id)
        {
            return await _context.Employees .AnyAsync(e => e.EmployeeId == id);
        }


        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Employees .AnyAsync(e => e.Email == email);
        }


        public async Task AddAsync(Employee employee)
        {
            await _context.Employees .AddAsync(employee);
        }
        
        
=======
        public async Task<bool> EmployeeExistsAsync(int id)
        {
            return await _context.Employees
                .AnyAsync(e => e.EmployeeId == id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Employees
                .AnyAsync(e => e.Email == email);
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
        }

>>>>>>> Stashed changes
        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
        }

<<<<<<< Updated upstream

        public async Task<decimal> GetMaxEmployeeIdAsync()
        {
            return await _context.Employees.MaxAsync(e => e.EmployeeId);
        }
        

=======
>>>>>>> Stashed changes
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
