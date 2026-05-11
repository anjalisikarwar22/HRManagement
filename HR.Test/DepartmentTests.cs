using Xunit;
using HRManagement.API.Controllers;
using HRManagement.API.DTOs.Departments;
using HRManagement.API.Exceptions;
using HRManagement.API.Interfaces;
using HRManagement.API.Models;
using HRManagement.API.Services;
using HRManagement.API.Validators;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HR.Test
{
    public class DepartmentTests
    {
        private readonly DepartmentValidator _validator = new DepartmentValidator();

        [Fact]
        public void Positive_ValidateCreate_WithValidData()
        {
            var dto = new CreateDepartmentDto
            {
                DepartmentName = "IT",
                ManagerId = 100,
                LocationId = 1700
            };

            var error = Record.Exception(() => _validator.ValidateCreate(dto));

            Assert.Null(error);
        }

        [Fact]
        public void Positive_ValidateUpdate_WithValidData()
        {
            var dto = new UpdateDepartmentDto
            {
                DepartmentName = "Finance",
                ManagerId = 200,
                LocationId = 2400
            };

            var error = Record.Exception(() => _validator.ValidateUpdate(dto));

            Assert.Null(error);
        }

        [Fact]
        public async Task Positive_CreateAsync_ReturnsCreatedDepartment()
        {
            var repo = new Mock<IDepartmentRepository>();
            repo.Setup(r => r.CreateAsync(It.IsAny<Department>()))
                .ReturnsAsync((Department d) =>
                {
                    d.DepartmentId = 30;
                    return d;
                });

            var service = new DepartmentService(repo.Object, _validator);
            var dto = new CreateDepartmentDto { DepartmentName = " Payroll " };

            var result = await service.CreateAsync(dto);

            Assert.Equal(30, result.DepartmentId);
            Assert.Equal("Payroll", result.DepartmentName);
        }

        [Fact]
        public async Task Positive_GetByIdAsync_ReturnsDepartment()
        {
            var repo = new Mock<IDepartmentRepository>();
            repo.Setup(r => r.GetByIdAsync(10))
                .ReturnsAsync(new Department { DepartmentId = 10, DepartmentName = "Admin" });

            var service = new DepartmentService(repo.Object, _validator);

            var result = await service.GetByIdAsync(10);

            Assert.NotNull(result);
            Assert.Equal("Admin", result!.DepartmentName);
        }

        [Fact]
        public void Negative_ValidateCreate_WhenNameIsTooLong()
        {
            var dto = new CreateDepartmentDto
            {
                DepartmentName = "This department name is too long"
            };

            Assert.Throws<ValidationException>(() => _validator.ValidateCreate(dto));
        }

        [Fact]
        public void Negative_ValidateCreate_WhenManagerIdHasDecimalValue()
        {
            var dto = new CreateDepartmentDto
            {
                DepartmentName = "Sales",
                ManagerId = 10.5m
            };

            Assert.Throws<ValidationException>(() => _validator.ValidateCreate(dto));
        }

        [Fact]
        public void Negative_ValidateUpdate_WhenLocationIdIsMoreThanFourDigits()
        {
            var dto = new UpdateDepartmentDto
            {
                DepartmentName = "Finance",
                LocationId = 10000
            };

            Assert.Throws<ValidationException>(() => _validator.ValidateUpdate(dto));
        }

        [Fact]
        public async Task Negative_Create_WhenValidationFails_ReturnsBadRequest()
        {
            var service = new Mock<IDepartmentService>();
            service.Setup(s => s.CreateAsync(It.IsAny<CreateDepartmentDto>()))
                .ThrowsAsync(new ValidationException("DepartmentName is required."));

            var controller = new DepartmentsController(service.Object);

            var result = await controller.Create(new CreateDepartmentDto());

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
