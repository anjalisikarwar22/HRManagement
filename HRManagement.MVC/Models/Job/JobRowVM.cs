namespace HRManagement.MVC.Models.Job;

public class JobRowVM
{
    public string JobId { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public decimal? SalarySpread => MaxSalary.HasValue && MinSalary.HasValue ? MaxSalary - MinSalary : null;
}
