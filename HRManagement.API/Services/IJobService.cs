using HRManagement.API.DTOs;

namespace HRManagement.API.Services
{
    public interface IJobService
    {
        Task<List<JobDTO>> GetAll();
        Task<int> Count();
        Task<JobDTO> GetById(string id);
        Task<List<JobDTO>> SearchByTitle(string title);
        Task<List<JobDTO>> GetBySalaryRange(decimal min, decimal max);
        Task<List<EmployeeDTO>> GetEmployees(string jobId);
        Task<JobDTO> Create(JobDTO dto);
        Task<JobDTO> Update(string id, JobDTO dto);
        Task<JobDTO> UpdateSalaryRange(string id, SalaryDTO dto);
    }
}
