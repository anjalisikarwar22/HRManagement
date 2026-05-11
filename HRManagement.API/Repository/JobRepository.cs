using HRManagement.API.Data;
using HRManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.API.Repository
{
    public class JobRepository : IJobRepository
    {
        private readonly HRContext _db;

        public JobRepository(HRContext db) => _db = db;

        public async Task<List<Job>> GetAll() => await _db.Jobs.ToListAsync();

        public async Task<int> Count() => await _db.Jobs.CountAsync();

        public async Task<Job?> GetById(string id) => await _db.Jobs.FindAsync(id);

        public async Task<List<Job>> SearchByTitle(string title)
            => await _db.Jobs.Where(j => j.JobTitle.Contains(title)).ToListAsync();

        public async Task<List<Job>> GetBySalaryRange(decimal min, decimal max)
            => await _db.Jobs
                .Where(j => j.MinSalary >= min && j.MaxSalary <= max)
                .ToListAsync();

        public async Task<List<Employee>> GetEmployees(string jobId)
            => await _db.Employees.Where(e => e.JobId == jobId).ToListAsync();

        public async Task Add(Job job)
        {
            _db.Jobs.Add(job);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Job job)
        {
            _db.Jobs.Update(job);
            await _db.SaveChangesAsync();
        }
    }
}
