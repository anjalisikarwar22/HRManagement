namespace HRManagement.API.DTOs.Employee
{
    public class EmployeeSummaryDto
    {
        public decimal EmployeeId { get; set; }

        public string FullName { get; set; }

        public string? Email  { get; set; }

        public decimal? JobTitle { get; set; }

        public decimal? DepartmentName { get; set; }
    }
}
