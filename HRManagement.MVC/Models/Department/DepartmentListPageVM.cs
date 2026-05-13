using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRManagement.MVC.Models.Department;

public class DepartmentListPageVM
{
    public List<DepartmentRowVM> Departments { get; set; } = new();
    public string? Search { get; set; }
    public decimal? LocationId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; } = 1;
    public DepartmentSummaryVM Summary { get; set; } = new();
    public List<SelectListItem> Locations { get; set; } = new();
    public string? StatusMessage { get; set; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

public class DepartmentSummaryVM
{
    public int TotalDepartments { get; set; }
    public int DepartmentsWithManager { get; set; }
    public int DepartmentsWithoutManager { get; set; }
    public int DepartmentsWithLocation { get; set; }
    public int DepartmentsWithoutLocation { get; set; }
    public double AverageEmployeesPerDepartment { get; set; }
}
