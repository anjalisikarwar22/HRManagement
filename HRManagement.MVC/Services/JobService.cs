using HRManagement.MVC.Models.Job;

namespace HRManagement.MVC.Services;

public class JobService : IJobService
{
    private readonly ApiClient _apiClient;

    public JobService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<JobIndexPageVM> GetIndexPageAsync(string? search, decimal? minSalary, decimal? maxSalary)
    {
        var endpoint = "api/jobs";
        if (!string.IsNullOrWhiteSpace(search))
        {
            endpoint = $"api/jobs/search?title={Uri.EscapeDataString(search)}";
        }
        else if (minSalary.HasValue || maxSalary.HasValue)
        {
            endpoint = $"api/jobs/by-salary-range?min={minSalary.GetValueOrDefault()}&max={maxSalary.GetValueOrDefault(decimal.MaxValue)}";
        }

        var response = await _apiClient.GetAsync<List<JobRowVM>>(endpoint);
        var countResponse = await _apiClient.GetAsync<int>("api/jobs/count");
        var jobs = response.Data ?? new List<JobRowVM>();

        return new JobIndexPageVM
        {
            Jobs = jobs,
            Search = search,
            MinSalary = minSalary,
            MaxSalary = maxSalary,
            TotalJobs = countResponse.Data > 0 ? countResponse.Data : jobs.Count,
            LowestSalary = jobs.Where(x => x.MinSalary.HasValue).Select(x => x.MinSalary!.Value).DefaultIfEmpty().Min(),
            HighestSalary = jobs.Where(x => x.MaxSalary.HasValue).Select(x => x.MaxSalary!.Value).DefaultIfEmpty().Max(),
            AverageBand = jobs.Where(x => x.SalarySpread.HasValue).Select(x => x.SalarySpread!.Value).DefaultIfEmpty().Average(),
            StatusMessage = response.Message
        };
    }

    public async Task<JobRowVM?> GetByIdAsync(string id)
    {
        var response = await _apiClient.GetAsync<JobRowVM>($"api/jobs/{Uri.EscapeDataString(id)}");
        return response.Success ? response.Data : null;
    }

    public async Task<(bool Success, string Message)> CreateAsync(CreateJobVM model)
    {
        var response = await _apiClient.PostAsync<CreateJobVM, JobRowVM>("api/jobs", model);
        return (response.Success, response.Message);
    }

    public async Task<(bool Success, string Message)> UpdateAsync(string id, CreateJobVM model)
    {
        var response = await _apiClient.PutAsync<CreateJobVM, JobRowVM>($"api/jobs/{Uri.EscapeDataString(id)}", model);
        return (response.Success, response.Message);
    }

    public async Task<(bool Success, string Message)> UpdateSalaryRangeAsync(string id, SalaryRangeVM model)
    {
        var response = await _apiClient.PatchAsync<SalaryRangeVM, JobRowVM>($"api/jobs/{Uri.EscapeDataString(id)}/salary-range", model);
        return (response.Success, response.Message);
    }
}
