namespace HRManagement.MVC.Models.Common;

public class DropdownOptionVM
{
    public decimal Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class EmployeeLookupVM
{
    public decimal EmployeeId { get; set; }
    public string FullName { get; set; } = string.Empty;
}
