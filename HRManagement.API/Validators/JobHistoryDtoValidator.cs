using FluentValidation;
using HRManagement.API.DTOs;

namespace HRManagement.API.Validators
{
    public class JobHistoryDtoValidator : AbstractValidator<JobHistoryDTO>
    {
        public JobHistoryDtoValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("EmployeeId is required.");

            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("JobId is required.")
                .Matches("^[A-Z0-9_]{2,10}$")
                .WithMessage("JobId must be 2-10 uppercase letters, digits, or underscores (e.g. IT_PROG).");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("StartDate is required.");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("EndDate must be on or after StartDate.");
        }
    }
}
