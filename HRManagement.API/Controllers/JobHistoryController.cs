using HRManagement.API.Common;
using HRManagement.API.DTOs;
using HRManagement.API.Exceptions;
using HRManagement.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers
{
    [Route("api/jobhistory")]
    [ApiController]
    public class JobHistoryController : ControllerBase
    {
        private readonly IJobHistoryService _service;

        public JobHistoryController(IJobHistoryService service) => _service = service;

        // GET /api/jobhistory
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

        // GET /api/jobhistory/count
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

        // GET /api/jobhistory/by-job/{jobId}   — jobId constrained to 1-10 chars
        [HttpGet("by-job/{jobId:length(1,10)}")]
        public async Task<IActionResult> ByJob(string jobId)
        {
            try
            {
                var data = await _service.GetByJob(jobId);
                return Ok(new ApiResponse<List<JobHistoryDTO>>
                {
                    Success = true,
                    Message = "Job history fetched successfully.",
                    Data = data
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<List<JobHistoryDTO>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null!
                });
            }
        }

        // GET /api/jobhistory/by-employee/{empId}   — must be a positive number
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

        // GET /api/jobhistory/by-department/{deptId}   — must be a positive number
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

        // POST /api/jobhistory
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JobHistoryDTO dto)
        {
            try
            {
                var data = await _service.Create(dto);
                return Created($"/api/jobhistory/by-employee/{data.EmployeeId}", new ApiResponse<JobHistoryDTO>
                {
                    Success = true,
                    Message = "Job history created successfully.",
                    Data = data
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<JobHistoryDTO>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null!
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiResponse<JobHistoryDTO>
                {
                    Success = false,
                    Message = string.Join(" | ", ex.Errors),
                    Data = null!
                });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ApiResponse<JobHistoryDTO>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null!
                });
            }
        }
    }
}
