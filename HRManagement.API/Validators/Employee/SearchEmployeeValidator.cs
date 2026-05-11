using FluentValidation;

namespace HRManagement.API.Validators.Employee
{
    public class SearchEmployeeValidator
        : AbstractValidator<string>
    {
        public SearchEmployeeValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .MinimumLength(2)
                .WithMessage(
                    "Search term must contain at least 2 characters");
        }
    }

}
