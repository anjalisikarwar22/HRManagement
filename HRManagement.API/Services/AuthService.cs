using HRManagement.API.Data;
using HRManagement.API.DTOs.Auth;
using HRManagement.API.Exceptions;
using HRManagement.API.Repository;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEmployeeRepository _employeeRepository;

        private readonly JwtService  _jwtService;

        private readonly HRContext _context;

        public AuthService(IEmployeeRepository employeeRepository, JwtService jwtService, HRContext context)
        {
            _employeeRepository =employeeRepository;

            _jwtService =jwtService;

            _context =context;
        }

        

        public async Task<string>
            RegisterAsync(RegisterDto dto)
        {
            var employee =await _context.Employees
                            .FirstOrDefaultAsync(e =>
                                e.EmployeeId == dto.EmployeeId &&
                                e.Email == dto.Email);

            if (employee == null)
            {
                throw new NotFoundException("Employee not found");
            }

            if (employee.Password != null)
            {
                throw new ValidationException("Account already activated");
            }

            employee.Password =BCrypt.Net.BCrypt.HashPassword(dto.Password);

            employee.Role = "Employee";

            await _context.SaveChangesAsync();

            return  "Account activated successfully";
        }

        

        public async Task<object> LoginAsync(LoginDto dto)
        {
            var employee = await _employeeRepository.GetByEmailAsync(dto.Email);

            if (employee == null)
            {
                throw new UnauthorizedException("Invalid email or password");
            }

            if (employee.Password == null)
            {
                throw new UnauthorizedException("Account not activated");
            }

            bool validPassword =BCrypt.Net.BCrypt.Verify( dto.Password,employee.Password);

            if (!validPassword)
            {
                throw new UnauthorizedException("Invalid email or password");
            }

            var token =_jwtService.GenerateToken(employee);

            return new
            {
                Token = token,

                employee.EmployeeId,

                employee.Email,

                employee.Role
            };
        }


        public async Task<object>
            GetCurrentUserAsync(decimal employeeId)
        {
            var employee =await _context.Employees
                            .Include(e => e.Job)
                            .Include(e => e.Department)
                            .Include(e => e.Manager)
                            .FirstOrDefaultAsync(e =>e.EmployeeId ==employeeId);

            if (employee == null)
            {
                throw new NotFoundException( "Employee not found");
            }

            decimal totalSalary =employee.Salary ?? 0;

            if (employee.CommissionPct != null)
            {
                totalSalary +=(employee.Salary ?? 0) * employee.CommissionPct.Value;
            }

            return new
            {
                employee.EmployeeId,

                FullName = employee.FirstName + " " + employee.LastName,

                employee.Email,

                JobTitle =employee.Job?.JobTitle,

                Department =employee.Department?.DepartmentName,

                employee.Salary,

                employee.CommissionPct,

                TotalSalary = totalSalary,

                Manager =employee.Manager != null? employee.Manager
                            .FirstName +" " +employee.Manager
                            .LastName: null,

                employee.Role
            };
        }

    }
}
