namespace HRManagement.API.DTOs.Employee
{
    public class CreateEmployeeDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string JobId { get; set; }

        public int? ManagerId { get; set; }

        public short? DepartmentId { get; set; }

        public decimal? Salary { get; set; }

        public decimal? CommissionPct { get; set; }
    }

}
