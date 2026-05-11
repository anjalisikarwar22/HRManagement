using FluentValidation;
using HRManagement.API.DTOs.Employee;

namespace HRManagement.API.Validators.Employee
{
    public class UpdateEmployeeValidator
        : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.JobId)
                .NotEmpty();

            RuleFor(x => x.DepartmentId)
                .NotNull();

            RuleFor(x => x.Salary)
                .GreaterThan(0);

            RuleFor(x => x.CommissionPct)
                .InclusiveBetween(0, 1)
                .When(x => x.CommissionPct.HasValue);

            RuleFor(x => x.Role)
                .Must(role =>
                    role == "Admin" ||
                    role == "Employee")
                .WithMessage(
                    "Role must be Admin or Employee");
        }
    }

}
