using Xunit;
using FluentValidation;
using FluentValidation.Results;
using HRManagement.API.DTOs;
using HRManagement.API.Exceptions;
using HRManagement.API.Models;
using HRManagement.API.Repository;
using HRManagement.API.Services;
using Moq;

namespace HR.Test
{
    public class JobServiceTests
    {
        private readonly Mock<IJobRepository> _repo;
        private readonly Mock<IValidator<JobDTO>> _jobValidator;
        private readonly Mock<IValidator<SalaryDTO>> _salaryValidator;
        private readonly JobService _service;

        public JobServiceTests()
        {
            _repo = new Mock<IJobRepository>();
            _jobValidator = new Mock<IValidator<JobDTO>>();
            _salaryValidator = new Mock<IValidator<SalaryDTO>>();

            _jobValidator.Setup(v => v.ValidateAsync(It.IsAny<JobDTO>(), default))
                .ReturnsAsync(new ValidationResult());
            _salaryValidator.Setup(v => v.ValidateAsync(It.IsAny<SalaryDTO>(), default))
                .ReturnsAsync(new ValidationResult());

            _service = new JobService(_repo.Object, _jobValidator.Object, _salaryValidator.Object);
        }

[Fact]
        public async Task GetAll_returns_all_jobs()
        {
            var jobs = new List<Job>
            {
                new Job { JobId = "IT_PROG", JobTitle = "Programmer" },
                new Job { JobId = "SA_MAN", JobTitle = "Sales Manager" }
            };
            _repo.Setup(r => r.GetAll()).ReturnsAsync(jobs);

            var result = await _service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal("Programmer", result[0].JobTitle);
        }

        [Fact]
        public async Task GetById_returns_job_when_found()
        {
            var job = new Job { JobId = "IT_PROG", JobTitle = "Programmer", MinSalary = 4000, MaxSalary = 10000 };
            _repo.Setup(r => r.GetById("IT_PROG")).ReturnsAsync(job);

            var result = await _service.GetById("IT_PROG");

            Assert.Equal("IT_PROG", result.JobId);
            Assert.Equal("Programmer", result.JobTitle);
        }

        [Fact]
        public async Task Count_returns_total()
        {
            _repo.Setup(r => r.Count()).ReturnsAsync(5);

            var result = await _service.Count();

            Assert.Equal(5, result);
        }

        [Fact]
        public async Task Create_saves_new_job()
        {
            var dto = new JobDTO { JobId = "TEST_J", JobTitle = "Tester", MinSalary = 3000, MaxSalary = 6000 };
            _repo.Setup(r => r.GetById("TEST_J")).ReturnsAsync((Job?)null);
            _repo.Setup(r => r.Add(It.IsAny<Job>())).Returns(Task.CompletedTask);

            var result = await _service.Create(dto);

            Assert.Equal("TEST_J", result.JobId);
            _repo.Verify(r => r.Add(It.IsAny<Job>()), Times.Once);
        }

[Fact]
        public async Task GetById_throws_NotFound_when_missing()
        {
            _repo.Setup(r => r.GetById("NOPE")).ReturnsAsync((Job?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetById("NOPE"));
        }

        [Fact]
        public async Task Create_throws_BadRequest_when_duplicate()
        {
            var dto = new JobDTO { JobId = "IT_PROG", JobTitle = "Programmer", MinSalary = 4000, MaxSalary = 10000 };
            _repo.Setup(r => r.GetById("IT_PROG"))
                .ReturnsAsync(new Job { JobId = "IT_PROG", JobTitle = "Programmer" });

            await Assert.ThrowsAsync<BadRequestException>(() => _service.Create(dto));
        }

        [Fact]
        public async Task Create_throws_Validation_when_input_invalid()
        {
            var dto = new JobDTO { JobId = "", JobTitle = "" };
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("JobId", "JobId is required."),
                new ValidationFailure("JobTitle", "JobTitle is required.")
            };
            _jobValidator.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult(failures));

            await Assert.ThrowsAsync<HRManagement.API.Exceptions.ValidationException>(
                () => _service.Create(dto));
        }

        [Fact]
        public async Task Update_throws_NotFound_when_missing()
        {
            var dto = new JobDTO { JobId = "X", JobTitle = "X", MinSalary = 1000, MaxSalary = 2000 };
            _repo.Setup(r => r.GetById("X")).ReturnsAsync((Job?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.Update("X", dto));
        }
    }
}
