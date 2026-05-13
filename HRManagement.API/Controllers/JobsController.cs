using HRManagement.API.Common;
using HRManagement.API.DTOs;
using HRManagement.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _service;

        public JobsController(IJobService service) => _service = service;
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ApiResponse<List<JobDTO>>>> GetAll()
        {
            var data = await _service.GetAll();
            return Ok(new ApiResponse<List<JobDTO>>
            {
                Success = true,
                Message = "Jobs fetched successfully.",
                Data = data
            });
        }
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
        [HttpGet("dropdown")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ApiResponse<object>>> Dropdown()
        {
            var data = await _service.GetDropdown();
            return Ok(new ApiResponse<object>(
                true,
                "Job dropdown fetched successfully.",
                data));
        }

        [HttpGet("count")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ApiResponse<int>>> Count()
        {
            var data = await _service.Count();
            return Ok(new ApiResponse<int>
            {
                Success = true,
                Message = "Job count fetched successfully.",
                Data = data
            });
        }
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
        [HttpGet("search")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ApiResponse<List<JobDTO>>>> Search([FromQuery] string title)
        {
            var data = await _service.SearchByTitle(title);
            return Ok(new ApiResponse<List<JobDTO>>
            {
                Success = true,
                Message = "Search completed.",
                Data = data
            });
        }
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
        [HttpGet("by-salary-range")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ApiResponse<List<JobDTO>>>> BySalaryRange(
            [FromQuery] decimal min,
            [FromQuery] decimal max)
        {
            var data = await _service.GetBySalaryRange(min, max);
            return Ok(new ApiResponse<List<JobDTO>>
            {
                Success = true,
                Message = "Jobs filtered by salary range.",
                Data = data
            });
        }
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
        [HttpGet("{id:length(1,10)}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ApiResponse<JobDTO>>> GetById(string id)
        {
            var data = await _service.GetById(id);
            return Ok(new ApiResponse<JobDTO>
            {
                Success = true,
                Message = "Job fetched successfully.",
                Data = data
            });
        }
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
        [HttpGet("{id:length(1,10)}/employees")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<EmployeeDTO>>>> Employees(string id)
        {
            var data = await _service.GetEmployees(id);
            return Ok(new ApiResponse<List<EmployeeDTO>>
            {
                Success = true,
                Message = "Employees fetched successfully.",
                Data = data
            });
        }
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<JobDTO>>> Create([FromBody] JobDTO dto)
        {
            var data = await _service.Create(dto);
            return Created($"/api/jobs/{data.JobId}", new ApiResponse<JobDTO>
            {
                Success = true,
                Message = "Job created successfully.",
                Data = data
            });
        }
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
        [HttpPut("{id:length(1,10)}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<JobDTO>>> Update(string id, [FromBody] JobDTO dto)
        {
            var data = await _service.Update(id, dto);
            return Ok(new ApiResponse<JobDTO>
            {
                Success = true,
                Message = "Job updated successfully.",
                Data = data
            });
        }
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
        [HttpPatch("{id:length(1,10)}/salary-range")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<JobDTO>>> UpdateSalaryRange(string id, [FromBody] SalaryDTO dto)
        {
            var data = await _service.UpdateSalaryRange(id, dto);
            return Ok(new ApiResponse<JobDTO>
            {
                Success = true,
                Message = "Salary range updated successfully.",
                Data = data
            });
        }
    }
}
<<<<<<< Updated upstream
=======


>>>>>>> Stashed changes
