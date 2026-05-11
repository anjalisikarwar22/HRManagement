namespace HRManagement.API.DTOs.Employee
{
    public class UpdateEmployeeDto
    {
        public string Email { get; set; }

        public string JobId { get; set; }

        public short? DepartmentId { get; set; }

        public int? ManagerId { get; set; }

        public decimal Salary { get; set; }

        public decimal? CommissionPct { get; set; }

        public string Role { get; set; }
    }

}
