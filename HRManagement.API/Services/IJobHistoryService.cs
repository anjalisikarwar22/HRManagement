using HRManagement.API.DTOs;

namespace HRManagement.API.Services
{
    public interface IJobHistoryService
    {
        Task<List<JobHistoryDTO>> GetAll();
        Task<int> Count();
        Task<List<JobHistoryDTO>> GetByJob(string jobId);
        Task<List<JobHistoryDTO>> GetByEmployee(decimal empId);
        Task<List<JobHistoryDTO>> GetByDepartment(decimal deptId);
        Task<JobHistoryDTO> Create(JobHistoryDTO dto);
        Task<List<JobHistoryDTO>> GetDropdown();
    }
}
