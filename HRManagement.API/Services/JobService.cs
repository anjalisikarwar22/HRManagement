using FluentValidation;
using HRManagement.API.DTOs;
using HRManagement.API.Exceptions;
using HRManagement.API.Models;
using HRManagement.API.Repository;
using ValidationException = HRManagement.API.Exceptions.ValidationException;

namespace HRManagement.API.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _repo;
        private readonly IValidator<JobDTO> _jobValidator;
        private readonly IValidator<SalaryDTO> _salaryValidator;

        public JobService(IJobRepository repo, IValidator<JobDTO> jobValidator, IValidator<SalaryDTO> salaryValidator)
        {
            _repo = repo;
            _jobValidator = jobValidator;
            _salaryValidator = salaryValidator;
        }

        public async Task<List<JobDTO>> GetAll()
        {
            var jobs = await _repo.GetAll();
            return jobs.Select(ToDto).ToList();
        }

        public Task<int> Count() => _repo.Count();

        public async Task<JobDTO> GetById(string id)
        {
            var job = await _repo.GetById(id) ?? throw new NotFoundException($"Job '{id}' not found.");
            return ToDto(job);
        }

        public async Task<List<JobDTO>> SearchByTitle(string title)
        {
            var jobs = await _repo.SearchByTitle(title ?? "");
            return jobs.Select(ToDto).ToList();
        }

        public async Task<List<JobDTO>> GetBySalaryRange(decimal min, decimal max)
        {
            var jobs = await _repo.GetBySalaryRange(min, max);
            return jobs.Select(ToDto).ToList();
        }

        public async Task<List<EmployeeDTO>> GetEmployees(string jobId)
        {
            var job = await _repo.GetById(jobId) ?? throw new NotFoundException($"Job '{jobId}' not found.");
            var employees = await _repo.GetEmployees(jobId);
            return employees.Select(ToEmployeeDto).ToList();
        }

        public async Task<JobDTO> Create(JobDTO dto)
        {
            await Validate(dto, _jobValidator);

            if (await _repo.GetById(dto.JobId) != null)
                throw new BadRequestException($"Job '{dto.JobId}' already exists.");

            var job = new Job
            {
                JobId = dto.JobId,
                JobTitle = dto.JobTitle,
                MinSalary = dto.MinSalary,
                MaxSalary = dto.MaxSalary
            };
            await _repo.Add(job);
            return ToDto(job);
        }

        public async Task<JobDTO> Update(string id, JobDTO dto)
        {
            await Validate(dto, _jobValidator);

            var job = await _repo.GetById(id) ?? throw new NotFoundException($"Job '{id}' not found.");
            job.JobTitle = dto.JobTitle;
            job.MinSalary = dto.MinSalary;
            job.MaxSalary = dto.MaxSalary;
            await _repo.Update(job);
            return ToDto(job);
        }

        public async Task<JobDTO> UpdateSalaryRange(string id, SalaryDTO dto)
        {
            await Validate(dto, _salaryValidator);

            var job = await _repo.GetById(id) ?? throw new NotFoundException($"Job '{id}' not found.");
            job.MinSalary = dto.MinSalary;
            job.MaxSalary = dto.MaxSalary;
            await _repo.Update(job);
            return ToDto(job);
        }

private static async Task Validate<T>(T dto, IValidator<T> validator)
        {
            var result = await validator.ValidateAsync(dto);
            if (!result.IsValid)
                throw new ValidationException(result.Errors.Select(e => e.ErrorMessage));
        }

        private static JobDTO ToDto(Job j) => new()
        {
            JobId = j.JobId,
            JobTitle = j.JobTitle,
            MinSalary = j.MinSalary,
            MaxSalary = j.MaxSalary
        };

        private static EmployeeDTO ToEmployeeDto(Employee e) => new()
        {
            EmployeeId = e.EmployeeId,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            JobId = e.JobId,
            Salary = e.Salary,
            DepartmentId = e.DepartmentId
        };
    }
}
