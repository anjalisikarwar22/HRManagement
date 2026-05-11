using HRManagement.API.Common;
using HRManagement.API.DTOs;
using HRManagement.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers
{
    [Route("api/jobhistory")]
    [ApiController]
    public class JobHistoryController : ControllerBase
    {
        private readonly IJobHistoryService _service;

        public JobHistoryController(IJobHistoryService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAll();
            return Ok(new ApiResponse<List<JobHistoryDTO>>
            {
                Success = true,
                Message = "Job histories fetched successfully.",
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
                Message = "Job history count fetched successfully.",
                Data = data
            });
        }

        [HttpGet("by-job/{jobId:length(1,10)}")]
        public async Task<IActionResult> ByJob(string jobId)
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
        public async Task<IActionResult> ByEmployee(decimal empId)
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
        public async Task<IActionResult> ByDepartment(decimal deptId)
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
        public async Task<IActionResult> Create([FromBody] JobHistoryDTO dto)
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
