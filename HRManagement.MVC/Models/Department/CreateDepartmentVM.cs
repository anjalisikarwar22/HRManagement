using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRManagement.MVC.Models.Department;

public class CreateDepartmentVM
{
    [Required, StringLength(30)]
    [Display(Name = "Department Name")]
    public string DepartmentName { get; set; } = string.Empty;

    [Display(Name = "Manager")]
    public decimal? ManagerId { get; set; }

    [Display(Name = "Location")]
    public decimal? LocationId { get; set; }

    public List<SelectListItem> ManagerOptions { get; set; } = new();
    public List<SelectListItem> LocationOptions { get; set; } = new();
}
