using HRManagement.API.ApiResponses;
using HRManagement.API.DTOs.Departments;
using HRManagement.API.Exceptions;
using HRManagement.API.Filters;
using HRManagement.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers
{
    [ApiController]
    [Route("api/departments")]
    [Produces("application/json")]
    [ServiceFilter(typeof(DepartmentHeaderFilter))]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DepartmentListDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<DepartmentListDto>>.SuccessResponse(
                departments, "Departments retrieved successfully."));
        }

        [HttpGet("{id:int:min(1):max(9999)}")]
        [ProducesResponseType(typeof(ApiResponse<DepartmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);

            if (department == null)
                return NotFound(ApiResponse<object>.FailureResponse($"Department {id} was not found."));

            return Ok(ApiResponse<DepartmentDto>.SuccessResponse(
                department, "Department retrieved successfully."));
        }

        [HttpGet("location/{locationId:int:min(1):max(9999)}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DepartmentListDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByLocation(int locationId)
        {
            var departments = await _departmentService.GetByLocationAsync(locationId);
            return Ok(ApiResponse<IEnumerable<DepartmentListDto>>.SuccessResponse(
                departments, $"Departments for location {locationId} retrieved successfully."));
        }

        [HttpGet("manager/{managerId:int:min(1):max(999999)}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DepartmentListDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByManager(int managerId)
        {
            var departments = await _departmentService.GetByManagerAsync(managerId);
            return Ok(ApiResponse<IEnumerable<DepartmentListDto>>.SuccessResponse(
                departments, $"Departments for manager {managerId} retrieved successfully."));
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DepartmentListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var departments = await _departmentService.SearchByNameAsync(name);
            return Ok(ApiResponse<IEnumerable<DepartmentListDto>>.SuccessResponse(
                departments, "Search completed successfully."));
        }

        [HttpGet("paged")]
        [ProducesResponseType(typeof(ApiResponse<PagedDepartmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _departmentService.GetPagedAsync(pageNumber, pageSize);
            return Ok(ApiResponse<PagedDepartmentDto>.SuccessResponse(
                result, "Paged departments retrieved successfully."));
        }

        [HttpGet("summary")]
        [ProducesResponseType(typeof(ApiResponse<DepartmentSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _departmentService.GetSummaryAsync();
            return Ok(ApiResponse<DepartmentSummaryDto>.SuccessResponse(
                summary, "Department summary retrieved successfully."));
        }

        [HttpGet("count")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCount()
        {
            var count = await _departmentService.GetCountAsync();
            return Ok(ApiResponse<int>.SuccessResponse(count, $"Total departments: {count}"));
        }

        [HttpGet("dropdown")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DepartmentDropdownDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDropdown()
        {
            var items = await _departmentService.GetDropdownAsync();
            return Ok(ApiResponse<IEnumerable<DepartmentDropdownDto>>.SuccessResponse(
                items, "Dropdown data retrieved successfully."));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<DepartmentDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
        {
            try
            {
                var created = await _departmentService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.DepartmentId },
                    ApiResponse<DepartmentDto>.SuccessResponse(created, "Department created successfully."));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(ex.Message));
            }
        }

        [HttpPut("{id:int:min(1):max(9999)}")]
        [ProducesResponseType(typeof(ApiResponse<DepartmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentDto dto)
        {
            try
            {
                var updated = await _departmentService.UpdateAsync(id, dto);

                if (updated == null)
                    return NotFound(ApiResponse<object>.FailureResponse($"Department {id} was not found."));

                return Ok(ApiResponse<DepartmentDto>.SuccessResponse(
                    updated, "Department updated successfully."));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ApiResponse<object>.FailureResponse(ex.Message));
            }
        }
    }
}
