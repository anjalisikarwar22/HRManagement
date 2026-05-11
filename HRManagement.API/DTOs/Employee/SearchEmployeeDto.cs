namespace HRManagement.API.DTOs.Employee
{
    public class SearchEmployeeDto
    {
        public decimal EmployeeId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string DepartmentName { get; set; }

        public string JobTitle { get; set; }
    }

}
