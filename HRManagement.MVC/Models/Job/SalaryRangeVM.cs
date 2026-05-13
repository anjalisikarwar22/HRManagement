using System.ComponentModel.DataAnnotations;

namespace HRManagement.MVC.Models.Job;

public class SalaryRangeVM
{
    [Display(Name = "Minimum Salary")]
    [Range(0, 99999999)]
    public decimal? MinSalary { get; set; }

    [Display(Name = "Maximum Salary")]
    [Range(0, 99999999)]
    public decimal? MaxSalary { get; set; }
}
