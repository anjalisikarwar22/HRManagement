using System.ComponentModel.DataAnnotations;

namespace HRManagement.MVC.Models.Job;

public class CreateJobVM
{
    [Required, StringLength(10)]
    [Display(Name = "Job ID")]
    public string JobId { get; set; } = string.Empty;

    [Required, StringLength(35)]
    [Display(Name = "Job Title")]
    public string JobTitle { get; set; } = string.Empty;

    [Display(Name = "Minimum Salary")]
    [Range(0, 99999999)]
    public decimal? MinSalary { get; set; }

    [Display(Name = "Maximum Salary")]
    [Range(0, 99999999)]
    public decimal? MaxSalary { get; set; }
}
