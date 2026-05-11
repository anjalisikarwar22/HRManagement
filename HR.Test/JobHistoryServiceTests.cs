using FluentValidation;
using FluentValidation.Results;
using HRManagement.API.DTOs;
using HRManagement.API.Exceptions;
using HRManagement.API.Models;
using HRManagement.API.Repository;
using HRManagement.API.Services;
using Moq;
using Xunit;

namespace HR.Test
{
    public class JobHistoryServiceTests
    {
        private readonly Mock<IJobHistoryRepository> _repo;
        private readonly Mock<IJobRepository> _jobRepo;
        private readonly Mock<IValidator<JobHistoryDTO>> _validator;
        private readonly JobHistoryService _service;

        public JobHistoryServiceTests()
        {
            _repo = new Mock<IJobHistoryRepository>();
            _jobRepo = new Mock<IJobRepository>();
            _validator = new Mock<IValidator<JobHistoryDTO>>();

            _validator.Setup(v => v.ValidateAsync(It.IsAny<JobHistoryDTO>(), default))
                .ReturnsAsync(new ValidationResult());

            _service = new JobHistoryService(_repo.Object, _jobRepo.Object, _validator.Object);
        }

        private static JobHistory SampleHistory() => new JobHistory
        {
            EmployeeId = 176,
            StartDate = new DateOnly(2020, 1, 15),
            EndDate = new DateOnly(2023, 6, 30),
            JobId = "IT_PROG",
            DepartmentId = 60
        };

        // ----- POSITIVE -----

        [Fact]
        public async Task GetAll_returns_all_records()
        {
            var list = new List<JobHistory> { SampleHistory(), SampleHistory() };
            _repo.Setup(r => r.GetAll()).ReturnsAsync(list);

            var result = await _service.GetAll();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByEmployee_returns_history_for_employee()
        {
            _repo.Setup(r => r.GetByEmployee(176))
                .ReturnsAsync(new List<JobHistory> { SampleHistory() });

            var result = await _service.GetByEmployee(176);

            Assert.Single(result);
            Assert.Equal(176, result[0].EmployeeId);
        }

        [Fact]
        public async Task GetByJob_returns_history_when_job_exists()
        {
            _jobRepo.Setup(r => r.GetById("IT_PROG"))
                .ReturnsAsync(new Job { JobId = "IT_PROG", JobTitle = "Programmer" });
            _repo.Setup(r => r.GetByJob("IT_PROG"))
                .ReturnsAsync(new List<JobHistory> { SampleHistory() });

            var result = await _service.GetByJob("IT_PROG");

            Assert.Single(result);
            Assert.Equal("IT_PROG", result[0].JobId);
        }

        [Fact]
        public async Task Create_saves_new_history()
        {
            var dto = new JobHistoryDTO
            {
                EmployeeId = 200,
                StartDate = new DateOnly(2021, 3, 1),
                EndDate = new DateOnly(2022, 12, 31),
                JobId = "IT_PROG",
                DepartmentId = 60
            };
            _jobRepo.Setup(r => r.GetById("IT_PROG"))
                .ReturnsAsync(new Job { JobId = "IT_PROG", JobTitle = "Programmer" });
            _repo.Setup(r => r.GetByKey(dto.EmployeeId, dto.StartDate))
                .ReturnsAsync((JobHistory?)null);
            _repo.Setup(r => r.Add(It.IsAny<JobHistory>())).Returns(Task.CompletedTask);

            var result = await _service.Create(dto);

            Assert.Equal(200, result.EmployeeId);
            _repo.Verify(r => r.Add(It.IsAny<JobHistory>()), Times.Once);
        }

        // ----- NEGATIVE -----

        [Fact]
        public async Task GetByJob_throws_NotFound_when_job_missing()
        {
            _jobRepo.Setup(r => r.GetById("NOPE")).ReturnsAsync((Job?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByJob("NOPE"));
        }

        [Fact]
        public async Task Create_throws_NotFound_when_job_missing()
        {
            var dto = new JobHistoryDTO
            {
                EmployeeId = 200,
                StartDate = new DateOnly(2021, 3, 1),
                EndDate = new DateOnly(2022, 1, 1),
                JobId = "GHOST"
            };
            _jobRepo.Setup(r => r.GetById("GHOST")).ReturnsAsync((Job?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.Create(dto));
        }

        [Fact]
        public async Task Create_throws_BadRequest_when_duplicate()
        {
            var dto = new JobHistoryDTO
            {
                EmployeeId = 176,
                StartDate = new DateOnly(2020, 1, 15),
                EndDate = new DateOnly(2023, 6, 30),
                JobId = "IT_PROG"
            };
            _jobRepo.Setup(r => r.GetById("IT_PROG"))
                .ReturnsAsync(new Job { JobId = "IT_PROG", JobTitle = "Programmer" });
            _repo.Setup(r => r.GetByKey(dto.EmployeeId, dto.StartDate))
                .ReturnsAsync(SampleHistory());

            await Assert.ThrowsAsync<BadRequestException>(() => _service.Create(dto));
        }

        [Fact]
        public async Task Create_throws_Validation_when_input_invalid()
        {
            var dto = new JobHistoryDTO { EmployeeId = 0, JobId = "" };
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("EmployeeId", "EmployeeId is required."),
                new ValidationFailure("JobId", "JobId is required.")
            };
            _validator.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult(failures));

            await Assert.ThrowsAsync<HRManagement.API.Exceptions.ValidationException>(
                () => _service.Create(dto));
        }
    }
}
