using HRManagement.API.Models;

namespace HRManagement.API.Repository
{
    public interface IJobHistoryRepository
    {
        Task<List<JobHistory>> GetAll();
        Task<int> Count();
        Task<List<JobHistory>> GetByJob(string jobId);
        Task<List<JobHistory>> GetByEmployee(decimal empId);
        Task<List<JobHistory>> GetByDepartment(decimal deptId);
        Task<JobHistory?> GetByKey(decimal empId, DateOnly startDate);
        Task Add(JobHistory history);
    }
}
