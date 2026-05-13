using HRManagement.MVC.Models.Department;

namespace HRManagement.MVC.Services;

public interface IDepartmentService
{
    Task<DepartmentListPageVM> GetIndexPageAsync(string? search, decimal? locationId, int pageNumber, int pageSize);
    Task<CreateDepartmentVM> BuildCreateModelAsync(CreateDepartmentVM? model = null);
    Task<DepartmentRowVM?> GetByIdAsync(decimal id);
    Task<(bool Success, string Message)> CreateAsync(CreateDepartmentVM model);
    Task<(bool Success, string Message)> UpdateAsync(decimal id, CreateDepartmentVM model);
}
