using HRManagement.MVC.Filters;
using HRManagement.MVC.Models.Department;
using HRManagement.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.MVC.Controllers;

[HrOnly]
public class DepartmentsController : Controller
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index(string? search, decimal? locationId, int pageNumber = 1, int pageSize = 10)
    {
        var model = await _departmentService.GetIndexPageAsync(search, locationId, pageNumber, pageSize);
        return View(model);
    }

    public async Task<IActionResult> Details(decimal id)
    {
        var model = await _departmentService.GetByIdAsync(id);
        return model is null ? NotFound() : View(model);
    }

    public async Task<IActionResult> Create()
    {
        return View(await _departmentService.BuildCreateModelAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateDepartmentVM model)
    {
        if (!ModelState.IsValid) return View(await _departmentService.BuildCreateModelAsync(model));
        var result = await _departmentService.CreateAsync(model);
        if (result.Success)
        {
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, result.Message);
        return View(await _departmentService.BuildCreateModelAsync(model));
    }

    public async Task<IActionResult> Edit(decimal id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        if (department is null) return NotFound();
        var model = new CreateDepartmentVM
        {
            DepartmentName = department.DepartmentName,
            ManagerId = department.ManagerId,
            LocationId = department.LocationId
        };
        ViewData["DepartmentId"] = id;
        return View(await _departmentService.BuildCreateModelAsync(model));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(decimal id, CreateDepartmentVM model)
    {
        ViewData["DepartmentId"] = id;
        if (!ModelState.IsValid) return View(await _departmentService.BuildCreateModelAsync(model));
        var result = await _departmentService.UpdateAsync(id, model);
        if (result.Success)
        {
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, result.Message);
        return View(await _departmentService.BuildCreateModelAsync(model));
    }
}
