using FluentValidation;
using HRManagement.API.DTOs.Departments;

namespace HRManagement.API.Validators
{
    public class UpdateDepartmentFluentValidator : AbstractValidator<UpdateDepartmentDto>
    {
        public UpdateDepartmentFluentValidator()
        {
            RuleFor(d => d.DepartmentName)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(d => d.ManagerId)
                .GreaterThan(0)
                .LessThanOrEqualTo(999999)
                .Must(BeWholeNumber)
                .When(d => d.ManagerId.HasValue);

            RuleFor(d => d.LocationId)
                .GreaterThan(0)
                .LessThanOrEqualTo(9999)
                .Must(BeWholeNumber)
                .When(d => d.LocationId.HasValue);
        }

        private static bool BeWholeNumber(decimal? value)
        {
            return !value.HasValue || value.Value == decimal.Truncate(value.Value);
        }
    }
}
