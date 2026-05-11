using HRManagement.API.Data;
using HRManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.API.Repository
{
    public class EmployeeRepository
        : IEmployeeRepository
    {
        private readonly HRContext _context;

        public EmployeeRepository(
            HRContext context)
        {
            _context = context;
        }

<<<<<<< HEAD
        public async Task<List<Employee>>
=======
public async Task<List<Employee>>
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
            GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Job)
                .Include(e => e.Department)
                .Include(e => e.Manager)
                .ToListAsync();
        }

<<<<<<< HEAD
        public async Task<Employee?>
=======
public async Task<Employee?>
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
            GetByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Job)
                .Include(e => e.Department)
                .Include(e => e.Manager)
                .FirstOrDefaultAsync(e =>
                    e.EmployeeId == id);
        }

<<<<<<< HEAD
        public async Task<Employee?>
=======
public async Task<Employee?>
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
            GetByEmailAsync(string email)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e =>
                    e.Email == email);
        }

<<<<<<< HEAD
        public async Task<List<Employee>>
=======
public async Task<List<Employee>>
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
            GetManagersAsync()
        {
            return await _context.Employees
                .Where(e =>
                    _context.Employees
                    .Any(x =>
                        x.ManagerId ==
                        e.EmployeeId))
                .ToListAsync();
        }

<<<<<<< HEAD
        public async Task<List<Employee>>
=======
public async Task<List<Employee>>
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
            GetSubordinatesAsync(int managerId)
        {
            return await _context.Employees
                .Where(e =>
                    e.ManagerId == managerId)
                .Include(e => e.Job)
                .ToListAsync();
        }

<<<<<<< HEAD
        public async Task<List<Employee>>
=======
public async Task<List<Employee>>
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
            SearchEmployeesAsync(string name)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Job)
                .Where(e =>
                    (e.FirstName + " " +
                     e.LastName)
                    .ToLower()
                    .Contains(name.ToLower()))
                .ToListAsync();
        }

<<<<<<< HEAD
        public async Task<List<Employee>>
=======
public async Task<List<Employee>>
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
            GetEmployeesByDepartmentAsync(
                short departmentId)
        {
            return await _context.Employees
                .Where(e =>
                    e.DepartmentId ==
                    departmentId)
                .Include(e => e.Job)
                .ToListAsync();
        }

<<<<<<< HEAD
        public async Task<bool>
=======
public async Task<bool>
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
            EmployeeExistsAsync(int id)
        {
            return await _context.Employees
                .AnyAsync(e =>
                    e.EmployeeId == id);
        }

<<<<<<< HEAD
        public async Task<bool>
=======
public async Task<bool>
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
            EmailExistsAsync(string email)
        {
            return await _context.Employees
                .AnyAsync(e =>
                    e.Email == email);
        }

<<<<<<< HEAD
        public async Task AddAsync(
=======
public async Task AddAsync(
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
            Employee employee)
        {
            await _context.Employees
                .AddAsync(employee);
        }

<<<<<<< HEAD
        public void Update(Employee employee)
=======
public void Update(Employee employee)
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
        {
            _context.Employees.Update(employee);
        }

<<<<<<< HEAD
        public async Task SaveChangesAsync()
=======
public async Task SaveChangesAsync()
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
        {
            await _context.SaveChangesAsync();
        }
    }

}

