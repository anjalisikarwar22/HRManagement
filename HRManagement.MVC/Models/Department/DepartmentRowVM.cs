namespace HRManagement.MVC.Models.Department;

public class DepartmentRowVM
{
    public decimal DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public decimal? ManagerId { get; set; }
    public decimal? LocationId { get; set; }
    public string ManagerDisplay => ManagerId.HasValue ? $"Manager #{ManagerId:0}" : "Unassigned";
    public string LocationDisplay => LocationId.HasValue ? $"Location #{LocationId:0}" : "No location";
}
