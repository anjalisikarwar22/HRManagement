using HRManagement.API.Common;
using HRManagement.API.DTOs;
using HRManagement.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _service;

        public JobsController(IJobService service) => _service = service;
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAll();
            return Ok(new ApiResponse<List<JobDTO>>
            {
                Success = true,
                Message = "Jobs fetched successfully.",
                Data = data
            });
        }
        [HttpGet("count")]
        public async Task<IActionResult> Count()
        {
            var data = await _service.Count();
            return Ok(new ApiResponse<int>
            {
                Success = true,
                Message = "Job count fetched successfully.",
                Data = data
            });
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string title)
        {
            var data = await _service.SearchByTitle(title);
            return Ok(new ApiResponse<List<JobDTO>>
            {
                Success = true,
                Message = "Search completed.",
                Data = data
            });
        }
        [HttpGet("by-salary-range")]
        public async Task<IActionResult> BySalaryRange(
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
        [HttpGet("{id:length(1,10)}")]
        public async Task<IActionResult> GetById(string id)
        {
            var data = await _service.GetById(id);
            return Ok(new ApiResponse<JobDTO>
            {
                Success = true,
                Message = "Job fetched successfully.",
                Data = data
            });
        }
        [HttpGet("{id:length(1,10)}/employees")]
        public async Task<IActionResult> Employees(string id)
        {
            var data = await _service.GetEmployees(id);
            return Ok(new ApiResponse<List<EmployeeDTO>>
            {
                Success = true,
                Message = "Employees fetched successfully.",
                Data = data
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JobDTO dto)
        {
            var data = await _service.Create(dto);
            return Created($"/api/jobs/{data.JobId}", new ApiResponse<JobDTO>
            {
                Success = true,
                Message = "Job created successfully.",
                Data = data
            });
        }
        [HttpPut("{id:length(1,10)}")]
        public async Task<IActionResult> Update(string id, [FromBody] JobDTO dto)
        {
            var data = await _service.Update(id, dto);
            return Ok(new ApiResponse<JobDTO>
            {
                Success = true,
                Message = "Job updated successfully.",
                Data = data
            });
        }
        [HttpPatch("{id:length(1,10)}/salary-range")]
        public async Task<IActionResult> UpdateSalaryRange(string id, [FromBody] SalaryDTO dto)
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

