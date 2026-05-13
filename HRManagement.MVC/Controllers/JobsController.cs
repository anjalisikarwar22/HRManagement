using HRManagement.MVC.Filters;
using HRManagement.MVC.Models.Job;
using HRManagement.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.MVC.Controllers;

[HrOnly]
public class JobsController : Controller
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    public async Task<IActionResult> Index(string? search, decimal? minSalary, decimal? maxSalary)
    {
        var model = await _jobService.GetIndexPageAsync(search, minSalary, maxSalary);
        return View(model);
    }

    public async Task<IActionResult> Details(string id)
    {
        var model = await _jobService.GetByIdAsync(id);
        return model is null ? NotFound() : View(model);
    }

    public IActionResult Create() => View(new CreateJobVM());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateJobVM model)
    {
        ValidateSalaryBand(model.MinSalary, model.MaxSalary);
        if (!ModelState.IsValid) return View(model);
        var result = await _jobService.CreateAsync(model);
        if (result.Success)
        {
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, result.Message);
        return View(model);
    }

    public async Task<IActionResult> Edit(string id)
    {
        var job = await _jobService.GetByIdAsync(id);
        if (job is null) return NotFound();
        return View(new CreateJobVM
        {
            JobId = job.JobId,
            JobTitle = job.JobTitle,
            MinSalary = job.MinSalary,
            MaxSalary = job.MaxSalary
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, CreateJobVM model)
    {
        ValidateSalaryBand(model.MinSalary, model.MaxSalary);
        if (!ModelState.IsValid) return View(model);
        var result = await _jobService.UpdateAsync(id, model);
        if (result.Success)
        {
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, result.Message);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateSalaryRange(string id, SalaryRangeVM model, string? search, decimal? minSalary, decimal? maxSalary)
    {
        if (model.MinSalary.HasValue && model.MaxSalary.HasValue && model.MinSalary > model.MaxSalary)
        {
            TempData["Error"] = "Minimum salary cannot be greater than maximum salary.";
            return RedirectToAction(nameof(Index), new { search, minSalary, maxSalary });
        }

        var result = await _jobService.UpdateSalaryRangeAsync(id, model);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(Index), new { search, minSalary, maxSalary });
    }

    private void ValidateSalaryBand(decimal? minSalary, decimal? maxSalary)
    {
        if (minSalary.HasValue && maxSalary.HasValue && minSalary > maxSalary)
        {
            ModelState.AddModelError(nameof(CreateJobVM.MaxSalary), "Maximum salary must be greater than or equal to minimum salary.");
        }
    }
}
