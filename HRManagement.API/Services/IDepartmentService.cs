using HRManagement.API.DTOs.Departments;

namespace HRManagement.API.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentListDto>> GetAllAsync();

        Task<DepartmentDto?> GetByIdAsync(decimal departmentId);

        Task<IEnumerable<DepartmentListDto>> GetByLocationAsync(decimal locationId);

        Task<IEnumerable<DepartmentListDto>> GetByManagerAsync(decimal managerId);

        Task<IEnumerable<DepartmentListDto>> SearchByNameAsync(string name);

        Task<PagedDepartmentDto> GetPagedAsync(int pageNumber, int pageSize);

        Task<DepartmentSummaryDto> GetSummaryAsync();

        Task<int> GetCountAsync();

        Task<IEnumerable<DepartmentDropdownDto>> GetDropdownAsync();

        Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);

        Task<DepartmentDto?> UpdateAsync(decimal departmentId, UpdateDepartmentDto dto);
    }
}