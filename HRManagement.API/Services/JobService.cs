using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IValidator<JobDTO> _jobValidator;
        private readonly IValidator<SalaryDTO> _salaryValidator;

        public JobService(
            IJobRepository repo,
            IMapper mapper,
            IValidator<JobDTO> jobValidator,
            IValidator<SalaryDTO> salaryValidator)
        {
            _repo = repo;
            _mapper = mapper;
            _jobValidator = jobValidator;
            _salaryValidator = salaryValidator;
        }

        public async Task<List<JobDTO>> GetAll()
        {
            var jobs = await _repo.GetAll();
            return _mapper.Map<List<JobDTO>>(jobs);
        }

        public Task<int> Count() => _repo.Count();

        public async Task<JobDTO> GetById(string id)
        {
            var job = await _repo.GetById(id) ?? throw new NotFoundException($"Job '{id}' not found.");
            return _mapper.Map<JobDTO>(job);
        }

        public async Task<List<JobDTO>> SearchByTitle(string title)
        {
            var jobs = await _repo.SearchByTitle(title ?? "");
            return _mapper.Map<List<JobDTO>>(jobs);
        }

        public async Task<List<JobDTO>> GetBySalaryRange(decimal min, decimal max)
        {
            var jobs = await _repo.GetBySalaryRange(min, max);
            return _mapper.Map<List<JobDTO>>(jobs);
        }

        public async Task<List<EmployeeDTO>> GetEmployees(string jobId)
        {
            var job = await _repo.GetById(jobId) ?? throw new NotFoundException($"Job '{jobId}' not found.");
            var employees = await _repo.GetEmployees(jobId);
            return _mapper.Map<List<EmployeeDTO>>(employees);
        }

        public async Task<JobDTO> Create(JobDTO dto)
        {
            await Validate(dto, _jobValidator);

            if (await _repo.GetById(dto.JobId) != null)
                throw new BadRequestException($"Job '{dto.JobId}' already exists.");

            var job = _mapper.Map<Job>(dto);
            await _repo.Add(job);
            return _mapper.Map<JobDTO>(job);
        }

        public async Task<JobDTO> Update(string id, JobDTO dto)
        {
            await Validate(dto, _jobValidator);

            var job = await _repo.GetById(id) ?? throw new NotFoundException($"Job '{id}' not found.");
            job.JobTitle = dto.JobTitle;
            job.MinSalary = dto.MinSalary;
            job.MaxSalary = dto.MaxSalary;
            await _repo.Update(job);
            return _mapper.Map<JobDTO>(job);
        }

        public async Task<JobDTO> UpdateSalaryRange(string id, SalaryDTO dto)
        {
            await Validate(dto, _salaryValidator);

            var job = await _repo.GetById(id) ?? throw new NotFoundException($"Job '{id}' not found.");
            job.MinSalary = dto.MinSalary;
            job.MaxSalary = dto.MaxSalary;
            await _repo.Update(job);
            return _mapper.Map<JobDTO>(job);
        }

        public async Task<List<JobDTO>> GetDropdown()
        {
            var jobs = await _repo.GetAll();
            return _mapper.Map<List<JobDTO>>(jobs);
        }

        private static async Task Validate<T>(T dto, IValidator<T> validator)
        {
            var result = await validator.ValidateAsync(dto);
            if (!result.IsValid)
                throw new ValidationException(result.Errors.Select(e => e.ErrorMessage));
        }
    }
}
