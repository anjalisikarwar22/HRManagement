using HRManagement.API.Common;
using HRManagement.API.DTOs.Employee;
using HRManagement.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController( IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees =await _employeeService.GetAllEmployeesAsync();

            return Ok(
                new ApiResponse<object>(
                    true,
                    "Employees fetched successfully",
                    employees));
        }

        

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(decimal id)
        {
            var currentUserId =int.Parse( User.FindFirstValue(ClaimTypes.NameIdentifier));

            var currentRole =User.FindFirstValue(ClaimTypes.Role);

            var employee = await _employeeService.GetEmployeeByIdAsync(id,currentUserId,currentRole);

            return Ok(
                new ApiResponse<object>(
                    true,
                    "Employee fetched successfully",
                    employee));
        }

        

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeDto dto)
        {
            var employee = await _employeeService.CreateEmployeeAsync(dto);

            return Ok(
                new ApiResponse<object>(
                    true,
                    "Employee created successfully",
                    employee));
        }

        

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee( decimal id,UpdateEmployeeDto dto)
        {
            var message =await _employeeService.UpdateEmployeeAsync(id, dto);

            return Ok(
                new ApiResponse<object>(
                    true,
                    message,
                    null));
        }


        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var employeeId =int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var employee = await _employeeService.GetMyProfileAsync(employeeId);

            return Ok(
                new ApiResponse<object>(
                    true,
                    "Profile fetched successfully",
                    employee));
        }

        

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile( UpdateMyProfileDto dto)
        {
            var employeeId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var message =await _employeeService.UpdateMyProfileAsync(employeeId, dto);

            return Ok(
                new ApiResponse<object>(
                    true,
                    message,
                    null));
        }

       

        [HttpGet("my-manager")]
        public async Task<IActionResult> GetMyManager()
        {
            var employeeId =int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var manager = await _employeeService.GetMyManagerAsync(employeeId);

            return Ok(
                new ApiResponse<object>(
                    true,
                    "Manager fetched successfully",
                    manager));
        }

        

        [HttpGet("my-subordinates")]
        public async Task<IActionResult> GetMySubordinates()
        {
            var employeeId =int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var employees =await _employeeService.GetMySubordinatesAsync(employeeId);

            return Ok(
                new ApiResponse<object>(
                    true,
                    "Subordinates fetched successfully",
                    employees));
        }

        

        [Authorize(Roles = "Admin")]
        [HttpGet("managers")]
        public async Task<IActionResult> GetManagers()
        {
            var managers =await _employeeService.GetManagersAsync();

            return Ok(
                new ApiResponse<object>(
                    true,
                    "Managers fetched successfully",
                    managers));
        }

        

        [Authorize(Roles = "Admin")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchEmployees([FromQuery] string name)
        {
            var employees = await _employeeService.SearchEmployeesAsync(name);

            return Ok(
                new ApiResponse<object>(
                    true,
                    "Employees fetched successfully",
                    employees));
        }

        

        [Authorize(Roles = "Admin")]
        [HttpGet("by-department/{departmentId}")]
        public async Task<IActionResult> GetEmployeesByDepartment(short departmentId)
        {
            var employees =await _employeeService.GetEmployeesByDepartmentAsync(departmentId);

            return Ok(
                new ApiResponse<object>(
                    true,
                    "Employees fetched successfully",
                    employees));
        }
    }
}
