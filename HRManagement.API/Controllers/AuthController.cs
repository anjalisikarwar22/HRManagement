using HRManagement.API.Data;
using HRManagement.API.DTOs.Auth;
using HRManagement.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HRManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HRContext _context;
        private readonly JwtService _jwtService;

        public AuthController(
            HRContext context,
            JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // =========================
        // REGISTER
        // =========================

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e =>
                    e.EmployeeId == dto.EmployeeId &&
                    e.Email == dto.Email);

            if (employee == null)
            {
                return BadRequest(new
                {
                    message = "Employee not found"
                });
            }

            // already activated
            if (employee.Password != null)
            {
                return BadRequest(new
                {
                    message = "Account already activated"
                });
            }

            // hash password
            employee.Password =
                BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // default role
            employee.Role = "Employee";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Account activated successfully"
            });
        }

        // =========================
        // LOGIN
        // =========================

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e =>
                    e.Email == dto.Email);

            if (employee == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password"
                });
            }

            // account not activated
            if (employee.Password == null)
            {
                return Unauthorized(new
                {
                    message = "Account not activated"
                });
            }

            bool validPassword =
                BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    employee.Password);

            if (!validPassword)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password"
                });
            }

            var token =_jwtService.GenerateToken(employee);

            return Ok(new
            {
                token,
                employee.EmployeeId,
                employee.Email,
                employee.Role
            });
        }

        // =========================
        // CURRENT USER
        // =========================

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var employeeId =
                decimal.Parse(User.FindFirstValue(
                    ClaimTypes.NameIdentifier));

            var employee = await _context.Employees
                .Include(e => e.Job)
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e =>
                    e.EmployeeId == employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            decimal totalSalary = employee.Salary?? 0;

            if (employee.CommissionPct != null)
            {
                totalSalary +=employee.Salary ?? 0 * employee.CommissionPct.Value;
            }

            return Ok(new
            {
                employee.EmployeeId,

                FullName =employee.FirstName + " " + employee.LastName,

                employee.Email,

                JobTitle = employee.Job?.JobTitle,

                Department =employee.Department?.DepartmentName,

                employee.Salary,

                employee.CommissionPct,

                TotalSalary = totalSalary,

                employee.Role
            });
        }
    }
}
