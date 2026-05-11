using FluentValidation;
using HRManagement.API.DTOs.Employee;

namespace HRManagement.API.Validators.Employee
{
    public class CreateEmployeeValidator
        : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required")
                .MaximumLength(20);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required")
                .MaximumLength(25);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Invalid email format");

            RuleFor(x => x.JobId)
                .NotEmpty()
                .WithMessage("Job ID is required");

            RuleFor(x => x.DepartmentId)
                .NotNull()
                .WithMessage("Department ID is required");

            RuleFor(x => x.Salary)
                .GreaterThan(0)
                .When(x => x.Salary.HasValue)
                .WithMessage(
                    "Salary must be greater than 0");

            RuleFor(x => x.CommissionPct)
                .InclusiveBetween(0, 1)
                .When(x => x.CommissionPct.HasValue)
                .WithMessage(
                    "Commission must be between 0 and 1");
        }
    }
}
