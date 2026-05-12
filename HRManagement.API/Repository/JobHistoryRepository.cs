using HRManagement.API.Data;
using HRManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.API.Repository
{
    public class JobHistoryRepository : IJobHistoryRepository
    {
        private readonly HRContext _db;

        public JobHistoryRepository(HRContext db) => _db = db;

        public async Task<List<JobHistory>> GetAll() => await _db.JobHistories.ToListAsync();

        public async Task<int> Count() => await _db.JobHistories.CountAsync();

        public async Task<List<JobHistory>> GetByJob(string jobId)
            => await _db.JobHistories.Where(h => h.JobId == jobId).ToListAsync();

        public async Task<List<JobHistory>> GetByEmployee(decimal empId)
            => await _db.JobHistories.Where(h => h.EmployeeId == empId).ToListAsync();

        public async Task<List<JobHistory>> GetByDepartment(decimal deptId)
            => await _db.JobHistories.Where(h => h.DepartmentId == deptId).ToListAsync();

        public async Task<JobHistory?> GetByKey(decimal empId, DateOnly startDate)
            => await _db.JobHistories.FindAsync(empId, startDate);

        public async Task Add(JobHistory history)
        {
            _db.JobHistories.Add(history);
            await _db.SaveChangesAsync();
        }
        public async Task<JobHistory?> GetLatestHistoryAsync(decimal employeeId)
        {
            return await _db.JobHistories.Where(j => j.EmployeeId == employeeId)
                                                  .OrderByDescending(j => j.EndDate)
                                                  .FirstOrDefaultAsync();
        }
    }
}
