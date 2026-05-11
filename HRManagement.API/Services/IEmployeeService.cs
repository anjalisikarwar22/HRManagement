using HRManagement.API.DTOs.Employee;

namespace HRManagement.API.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponseDto>>
            GetAllEmployeesAsync();

        Task<EmployeeResponseDto>
            GetEmployeeByIdAsync(
                int id,
                int currentUserId,
                string currentRole);

        Task<EmployeeResponseDto>
            CreateEmployeeAsync(
                CreateEmployeeDto dto);

        Task<string>
            UpdateEmployeeAsync(
                int id,
                UpdateEmployeeDto dto);

Task<string>
            UpdateRoleAsync(
                int id,
                UpdateRoleDto dto);

        Task<EmployeeResponseDto>
            GetMyProfileAsync(int employeeId);

        Task<string>
            UpdateMyProfileAsync(
                int employeeId,
                UpdateMyProfileDto dto);

        Task<ManagerResponseDto>
            GetMyManagerAsync(int employeeId);

        Task<IEnumerable<SubordinateResponseDto>>
            GetMySubordinatesAsync(
                int employeeId);

        Task<IEnumerable<EmployeeLookupDto>>
            GetManagersAsync();

        Task<IEnumerable<SearchEmployeeDto>>
            SearchEmployeesAsync(string name);

        Task<IEnumerable<SearchEmployeeDto>>
            GetEmployeesByDepartmentAsync(
                short departmentId);
    }

}
