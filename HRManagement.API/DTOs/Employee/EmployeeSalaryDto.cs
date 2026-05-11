namespace HRManagement.API.DTOs.Employee
{
    public class EmployeeSalaryDto
    {
        public decimal Salary { get; set; }

        public decimal? CommissionPct { get; set; }

        public decimal TotalSalary { get; set; }
    }

}
