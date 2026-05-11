namespace HRManagement.API.DTOs.Employee
{
    public class EmployeeResponseDto
    {
        public decimal EmployeeId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string JobId { get; set; }

        public string JobTitle { get; set; }

        public decimal? DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public decimal? ManagerId { get; set; }

        public string ManagerName { get; set; }

        public decimal? Salary { get; set; }

        public decimal? CommissionPct { get; set; }

        public decimal TotalSalary { get; set; }

        public string Role { get; set; }

        public DateOnly HireDate{ get; set; }
    }
}
