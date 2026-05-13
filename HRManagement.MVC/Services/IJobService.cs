using HRManagement.MVC.Models.Job;

namespace HRManagement.MVC.Services;

public interface IJobService
{
    Task<JobIndexPageVM> GetIndexPageAsync(string? search, decimal? minSalary, decimal? maxSalary);
    Task<JobRowVM?> GetByIdAsync(string id);
    Task<(bool Success, string Message)> CreateAsync(CreateJobVM model);
    Task<(bool Success, string Message)> UpdateAsync(string id, CreateJobVM model);
    Task<(bool Success, string Message)> UpdateSalaryRangeAsync(string id, SalaryRangeVM model);
}
