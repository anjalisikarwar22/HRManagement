using HRManagement.API.DTOs.Departments;
using HRManagement.API.Models;

namespace HRManagement.API.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(decimal departmentId);
        Task<IEnumerable<Department>> GetByLocationAsync(decimal locationId);
        Task<IEnumerable<Department>> GetByManagerAsync(decimal managerId);
        Task<IEnumerable<Department>> SearchByNameAsync(string name);
        Task<(IEnumerable<Department> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<DepartmentSummaryDto> GetSummaryAsync();
        Task<int> GetCountAsync();
        Task<IEnumerable<Department>> GetDropdownAsync();
        Task<bool> ExistsAsync(decimal departmentId);
        Task<bool> NameExistsAsync(string name, decimal? excludeId = null);

        Task<Department> CreateAsync(Department department);
        Task<Department> UpdateAsync(Department department);
    }
}
