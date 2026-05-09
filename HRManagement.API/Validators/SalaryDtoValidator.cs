using FluentValidation;
using HRManagement.API.DTOs;

namespace HRManagement.API.Validators
{
    public class SalaryDtoValidator : AbstractValidator<SalaryDTO>
    {
        public SalaryDtoValidator()
        {
            RuleFor(x => x.MinSalary)
                .GreaterThanOrEqualTo(0).When(x => x.MinSalary.HasValue);

            RuleFor(x => x.MaxSalary)
                .GreaterThanOrEqualTo(0).When(x => x.MaxSalary.HasValue);

            RuleFor(x => x.MaxSalary)
                .GreaterThanOrEqualTo(x => x.MinSalary)
                .When(x => x.MinSalary.HasValue && x.MaxSalary.HasValue)
                .WithMessage("MaxSalary must be greater than or equal to MinSalary.");
        }
    }
}
