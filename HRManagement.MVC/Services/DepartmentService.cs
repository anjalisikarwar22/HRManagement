using HRManagement.MVC.Models.Common;
using HRManagement.MVC.Models.Department;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRManagement.MVC.Services;

public class DepartmentService : IDepartmentService
{
    private readonly ApiClient _apiClient;

    public DepartmentService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<DepartmentListPageVM> GetIndexPageAsync(string? search, decimal? locationId, int pageNumber, int pageSize)
    {
        pageNumber = Math.Max(pageNumber, 1);
        pageSize = Math.Clamp(pageSize, 5, 50);
        ApiResponseVM<PagedDepartmentsApiVM>? paged = null;
        ApiResponseVM<List<DepartmentRowVM>>? list = null;

        if (!string.IsNullOrWhiteSpace(search))
        {
            list = await _apiClient.GetAsync<List<DepartmentRowVM>>($"api/departments/search?name={Uri.EscapeDataString(search)}");
        }
        else if (locationId.HasValue)
        {
            list = await _apiClient.GetAsync<List<DepartmentRowVM>>($"api/departments/location/{locationId:0}");
        }
        else
        {
            paged = await _apiClient.GetAsync<PagedDepartmentsApiVM>($"api/departments/paged?pageNumber={pageNumber}&pageSize={pageSize}");
        }

        var summaryTask = _apiClient.GetAsync<DepartmentSummaryVM>("api/departments/summary");
        var locationsTask = GetLocationOptionsAsync(locationId);
        await Task.WhenAll(summaryTask, locationsTask);

        var rows = paged?.Data?.Items?.ToList() ?? list?.Data ?? new List<DepartmentRowVM>();
        var totalCount = paged?.Data?.TotalCount ?? rows.Count;
        var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));

        return new DepartmentListPageVM
        {
            Departments = rows,
            Search = search,
            LocationId = locationId,
            PageNumber = paged?.Data?.PageNumber ?? pageNumber,
            PageSize = paged?.Data?.PageSize ?? pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Summary = summaryTask.Result.Data ?? BuildSummary(rows),
            Locations = locationsTask.Result,
            StatusMessage = paged?.Message ?? list?.Message ?? summaryTask.Result.Message
        };
    }

    public async Task<CreateDepartmentVM> BuildCreateModelAsync(CreateDepartmentVM? model = null)
    {
        model ??= new CreateDepartmentVM();
        model.ManagerOptions = await GetManagerOptionsAsync(model.ManagerId);
        model.LocationOptions = await GetLocationOptionsAsync(model.LocationId);
        return model;
    }

    public async Task<DepartmentRowVM?> GetByIdAsync(decimal id)
    {
        var response = await _apiClient.GetAsync<DepartmentRowVM>($"api/departments/{id:0}");
        return response.Success ? response.Data : null;
    }

    public async Task<(bool Success, string Message)> CreateAsync(CreateDepartmentVM model)
    {
        var response = await _apiClient.PostAsync<CreateDepartmentRequestVM, DepartmentRowVM>("api/departments", ToRequest(model));
        return (response.Success, response.Message);
    }

    public async Task<(bool Success, string Message)> UpdateAsync(decimal id, CreateDepartmentVM model)
    {
        var response = await _apiClient.PutAsync<CreateDepartmentRequestVM, DepartmentRowVM>($"api/departments/{id:0}", ToRequest(model));
        return (response.Success, response.Message);
    }

    private async Task<List<SelectListItem>> GetLocationOptionsAsync(decimal? selected)
    {
        var response = await _apiClient.GetAsync<List<DropdownOptionVM>>("api/locations/dropdown");
        var items = new List<SelectListItem> { new("All Locations", string.Empty, !selected.HasValue) };
        items.AddRange((response.Data ?? new List<DropdownOptionVM>())
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.Id.ToString("0"), selected == x.Id)));
        return items;
    }

    private async Task<List<SelectListItem>> GetManagerOptionsAsync(decimal? selected)
    {
        var response = await _apiClient.GetAsync<List<EmployeeLookupVM>>("api/Employees/managers");
        var items = new List<SelectListItem> { new("Unassigned", string.Empty, !selected.HasValue) };
        items.AddRange((response.Data ?? new List<EmployeeLookupVM>())
            .OrderBy(x => x.FullName)
            .Select(x => new SelectListItem($"{x.FullName} (#{x.EmployeeId:0})", x.EmployeeId.ToString("0"), selected == x.EmployeeId)));
        return items;
    }

    private static DepartmentSummaryVM BuildSummary(IEnumerable<DepartmentRowVM> rows)
    {
        var list = rows.ToList();
        return new DepartmentSummaryVM
        {
            TotalDepartments = list.Count,
            DepartmentsWithManager = list.Count(x => x.ManagerId.HasValue),
            DepartmentsWithoutManager = list.Count(x => !x.ManagerId.HasValue),
            DepartmentsWithLocation = list.Count(x => x.LocationId.HasValue),
            DepartmentsWithoutLocation = list.Count(x => !x.LocationId.HasValue)
        };
    }

    private static CreateDepartmentRequestVM ToRequest(CreateDepartmentVM model) => new()
    {
        DepartmentName = model.DepartmentName,
        ManagerId = model.ManagerId,
        LocationId = model.LocationId
    };

    private class CreateDepartmentRequestVM
    {
        public string DepartmentName { get; set; } = string.Empty;
        public decimal? ManagerId { get; set; }
        public decimal? LocationId { get; set; }
    }

    private class PagedDepartmentsApiVM
    {
        public IEnumerable<DepartmentRowVM> Items { get; set; } = new List<DepartmentRowVM>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
