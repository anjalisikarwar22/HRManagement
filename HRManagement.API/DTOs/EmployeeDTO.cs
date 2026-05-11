namespace HRManagement.API.DTOs
{
    public class EmployeeDTO
    {
        public decimal EmployeeId { get; set; }

        public string? FirstName { get; set; }

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string JobId { get; set; } = string.Empty;

        public decimal? Salary { get; set; }

        public string? FirstName { get; set; }
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string JobId { get; set; } = null!;
        public decimal? Salary { get; set; }
        public decimal? DepartmentId { get; set; }
    }
}
