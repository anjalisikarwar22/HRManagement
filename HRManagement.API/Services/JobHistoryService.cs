using FluentValidation;
using HRManagement.API.DTOs;
using HRManagement.API.Exceptions;
using HRManagement.API.Models;
using HRManagement.API.Repository;
using ValidationException = HRManagement.API.Exceptions.ValidationException;

namespace HRManagement.API.Services
{
    public class JobHistoryService : IJobHistoryService
    {
        private readonly IJobHistoryRepository _repo;
        private readonly IJobRepository _jobRepo;
        private readonly IValidator<JobHistoryDTO> _validator;

        public JobHistoryService(IJobHistoryRepository repo, IJobRepository jobRepo, IValidator<JobHistoryDTO> validator)
        {
            _repo = repo;
            _jobRepo = jobRepo;
            _validator = validator;
        }

        public async Task<List<JobHistoryDTO>> GetAll()
        {
            var list = await _repo.GetAll();
            return list.Select(ToDto).ToList();
        }

        public Task<int> Count() => _repo.Count();

        public async Task<List<JobHistoryDTO>> GetByJob(string jobId)
        {
            var job = await _jobRepo.GetById(jobId) ?? throw new NotFoundException($"Job '{jobId}' not found.");
            var list = await _repo.GetByJob(jobId);
            return list.Select(ToDto).ToList();
        }

        public async Task<List<JobHistoryDTO>> GetByEmployee(decimal empId)
        {
            var list = await _repo.GetByEmployee(empId);
            return list.Select(ToDto).ToList();
        }

        public async Task<List<JobHistoryDTO>> GetByDepartment(decimal deptId)
        {
            var list = await _repo.GetByDepartment(deptId);
            return list.Select(ToDto).ToList();
        }

        public async Task<JobHistoryDTO> Create(JobHistoryDTO dto)
        {
            var result = await _validator.ValidateAsync(dto);
            if (!result.IsValid)
                throw new ValidationException(result.Errors.Select(e => e.ErrorMessage));

            if (await _jobRepo.GetById(dto.JobId) == null)
                throw new NotFoundException($"Job '{dto.JobId}' not found.");

            if (await _repo.GetByKey(dto.EmployeeId, dto.StartDate) != null)
                throw new BadRequestException("This job history record already exists.");

            var entity = new JobHistory
            {
                EmployeeId = dto.EmployeeId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                JobId = dto.JobId,
                DepartmentId = dto.DepartmentId
            };
            await _repo.Add(entity);
            return ToDto(entity);
        }

        private static JobHistoryDTO ToDto(JobHistory h) => new()
        {
            EmployeeId = h.EmployeeId,
            StartDate = h.StartDate,
            EndDate = h.EndDate,
            JobId = h.JobId,
            DepartmentId = h.DepartmentId
        };
    }
}
