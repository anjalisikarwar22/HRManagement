using HRManagement.API.Models;

namespace HRManagement.API.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();

        Task<Employee?> GetByIdAsync(int id);

        Task<Employee?> GetByEmailAsync(string email);

        Task<List<Employee>> GetManagersAsync();

        Task<List<Employee>> GetSubordinatesAsync(
            int managerId);

        Task<List<Employee>> SearchEmployeesAsync(
            string name);

        Task<List<Employee>>
            GetEmployeesByDepartmentAsync(
                short departmentId);

        Task<bool> EmployeeExistsAsync(int id);

        Task<bool> EmailExistsAsync(string email);

        Task AddAsync(Employee employee);

        void Update(Employee employee);

Task SaveChangesAsync();
    }

}
