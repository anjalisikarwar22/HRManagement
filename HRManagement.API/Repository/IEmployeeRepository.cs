using HRManagement.API.Models;

namespace HRManagement.API.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();

        Task<Employee?> GetByIdAsync(decimal id);

        Task<Employee?> GetByEmailAsync(string email);

        Task<List<Employee>> GetManagersAsync();

        Task<List<Employee>> GetSubordinatesAsync(decimal managerId);

        Task<List<Employee>> SearchEmployeesAsync(string name);

        Task<List<Employee>> GetEmployeesByDepartmentAsync(short departmentId);

        Task<bool> EmployeeExistsAsync(decimal id);

        Task<bool> EmailExistsAsync(string email);

        Task AddAsync(Employee employee);

        void Update(Employee employee);

        Task<decimal> GetMaxEmployeeIdAsync();

        Task SaveChangesAsync();
    }

}
