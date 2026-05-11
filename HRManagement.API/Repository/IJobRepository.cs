using HRManagement.API.Models;

namespace HRManagement.API.Repository
{
    public interface IJobRepository
    {
        Task<List<Job>> GetAll();
        Task<int> Count();
        Task<Job?> GetById(string id);
        Task<List<Job>> SearchByTitle(string title);
        Task<List<Job>> GetBySalaryRange(decimal min, decimal max);
        Task<List<Employee>> GetEmployees(string jobId);
        Task Add(Job job);
        Task Update(Job job);
    }
}
