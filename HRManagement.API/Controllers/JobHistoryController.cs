using HRManagement.API.Common;
using HRManagement.API.DTOs;
using HRManagement.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers
{
    [Route("api/jobhistory")]
    [ApiController]
    [Authorize]
    public class JobHistoryController : ControllerBase
    {
        private readonly IJobHistoryService _service;

        public JobHistoryController(IJobHistoryService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<JobHistoryDTO>>>> GetAll()
        {
            var data = await _service.GetAll();
            return Ok(new ApiResponse<List<JobHistoryDTO>>
            {
                Success = true,
                Message = "Job histories fetched successfully.",
                Data = data
            });
        }

        [HttpGet("dropdown")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> Dropdown()
        {
            var data = await _service.GetDropdown();
            return Ok(new ApiResponse<object>(
                true,
                "Job history dropdown fetched successfully.",
                data));
        }

        [HttpGet("count")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<int>>> Count()
        {
            var data = await _service.Count();
            return Ok(new ApiResponse<int>
            {
                Success = true,
                Message = "Job history count fetched successfully.",
                Data = data
            });
        }

        [HttpGet("by-job/{jobId:length(1,10)}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ApiResponse<List<JobHistoryDTO>>>> ByJob(string jobId)
        {
            var data = await _service.GetByJob(jobId);
            return Ok(new ApiResponse<List<JobHistoryDTO>>
            {
                Success = true,
                Message = "Job history fetched successfully.",
                Data = data
            });
        }

        [HttpGet("by-employee/{empId:decimal:min(1)}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ApiResponse<List<JobHistoryDTO>>>> ByEmployee(decimal empId)
        {
            var data = await _service.GetByEmployee(empId);
            return Ok(new ApiResponse<List<JobHistoryDTO>>
            {
                Success = true,
                Message = "Job history fetched successfully.",
                Data = data
            });
        }

        [HttpGet("by-department/{deptId:decimal:min(1)}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<JobHistoryDTO>>>> ByDepartment(decimal deptId)
        {
            var data = await _service.GetByDepartment(deptId);
            return Ok(new ApiResponse<List<JobHistoryDTO>>
            {
                Success = true,
                Message = "Job history fetched successfully.",
                Data = data
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<JobHistoryDTO>>> Create([FromBody] JobHistoryDTO dto)
        {
            var data = await _service.Create(dto);
            return Created($"/api/jobhistory/by-employee/{data.EmployeeId}", new ApiResponse<JobHistoryDTO>
            {
                Success = true,
                Message = "Job history created successfully.",
                Data = data
            });
        }
    }
}
