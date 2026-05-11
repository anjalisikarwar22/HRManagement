using HRManagement.API.DTOs.Employee;

namespace HRManagement.API.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync();

        Task<EmployeeResponseDto> GetEmployeeByIdAsync(decimal id, decimal currentUserId,string currentRole);

        Task<EmployeeResponseDto>  CreateEmployeeAsync(CreateEmployeeDto dto);

        Task<string> UpdateEmployeeAsync(decimal id,UpdateEmployeeDto dto);


        Task<EmployeeResponseDto> GetMyProfileAsync(decimal employeeId);

        Task<string> UpdateMyProfileAsync(decimal employeeId, UpdateMyProfileDto dto);

        Task<EmployeeLookupDto> GetMyManagerAsync(decimal employeeId);

        Task<IEnumerable<EmployeeSummaryDto>> GetMySubordinatesAsync(decimal employeeId);

        Task<IEnumerable<EmployeeLookupDto>> GetManagersAsync();

        Task<IEnumerable<EmployeeSummaryDto>> SearchEmployeesAsync(string name);

        Task<IEnumerable<EmployeeSummaryDto>> GetEmployeesByDepartmentAsync(short departmentId);
    }
}
