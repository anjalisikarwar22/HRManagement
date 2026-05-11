using FluentValidation;
using HRManagement.API.DTOs;

namespace HRManagement.API.Validators
{
    public class JobDtoValidator : AbstractValidator<JobDTO>
    {
        public JobDtoValidator()
        {
            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("JobId is required.")
                .MaximumLength(10);

            RuleFor(x => x.JobTitle)
                .NotEmpty().WithMessage("JobTitle is required.")
                .MaximumLength(35);

            RuleFor(x => x.MinSalary)
                .GreaterThanOrEqualTo(0).When(x => x.MinSalary.HasValue);

            RuleFor(x => x.MaxSalary)
                .GreaterThanOrEqualTo(x => x.MinSalary)
                .When(x => x.MinSalary.HasValue && x.MaxSalary.HasValue)
                .WithMessage("MaxSalary must be greater than or equal to MinSalary.");
        }
    }
}
