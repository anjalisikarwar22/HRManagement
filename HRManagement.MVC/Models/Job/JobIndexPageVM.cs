namespace HRManagement.MVC.Models.Job;

public class JobIndexPageVM
{
    public List<JobRowVM> Jobs { get; set; } = new();
    public string? Search { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public int TotalJobs { get; set; }
    public decimal? LowestSalary { get; set; }
    public decimal? HighestSalary { get; set; }
    public decimal? AverageBand { get; set; }
    public string? StatusMessage { get; set; }
}
