using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IValidator<JobHistoryDTO> _validator;

        public JobHistoryService(
            IJobHistoryRepository repo,
            IJobRepository jobRepo,
            IMapper mapper,
            IValidator<JobHistoryDTO> validator)
        {
            _repo = repo;
            _jobRepo = jobRepo;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<List<JobHistoryDTO>> GetAll()
        {
            var list = await _repo.GetAll();
            return _mapper.Map<List<JobHistoryDTO>>(list);
        }

        public Task<int> Count() => _repo.Count();

        public async Task<List<JobHistoryDTO>> GetByJob(string jobId)
        {
            var job = await _jobRepo.GetById(jobId) ?? throw new NotFoundException($"Job '{jobId}' not found.");
            var list = await _repo.GetByJob(jobId);
            return _mapper.Map<List<JobHistoryDTO>>(list);
        }

        public async Task<List<JobHistoryDTO>> GetByEmployee(decimal empId)
        {
            var list = await _repo.GetByEmployee(empId);
            return _mapper.Map<List<JobHistoryDTO>>(list);
        }

        public async Task<List<JobHistoryDTO>> GetByDepartment(decimal deptId)
        {
            var list = await _repo.GetByDepartment(deptId);
            return _mapper.Map<List<JobHistoryDTO>>(list);
        }

        public async Task<List<JobHistoryDTO>> GetDropdown()
        {
            var list = await _repo.GetAll();
            return _mapper.Map<List<JobHistoryDTO>>(list);
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

            var entity = _mapper.Map<JobHistory>(dto);
            await _repo.Add(entity);
            return _mapper.Map<JobHistoryDTO>(entity);
        }
    }
}
