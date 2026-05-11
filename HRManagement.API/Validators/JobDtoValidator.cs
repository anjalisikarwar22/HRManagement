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
                .Matches("^[A-Z0-9_]{2,10}$")
                .WithMessage("JobId must be 2-10 uppercase letters, digits, or underscores (e.g. IT_PROG).");

            RuleFor(x => x.JobTitle)
                .NotEmpty().WithMessage("JobTitle is required.")
                .MaximumLength(35)
                .Matches(@"^[A-Za-z\.\- ]+$")
                .WithMessage("JobTitle can contain only letters, spaces, dots, or hyphens.");

            RuleFor(x => x.MinSalary)
                .GreaterThanOrEqualTo(0).When(x => x.MinSalary.HasValue);

            RuleFor(x => x.MaxSalary)
                .GreaterThanOrEqualTo(x => x.MinSalary)
                .When(x => x.MinSalary.HasValue && x.MaxSalary.HasValue)
                .WithMessage("MaxSalary must be greater than or equal to MinSalary.");
        }
    }
}
