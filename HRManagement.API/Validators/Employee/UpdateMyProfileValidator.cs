using FluentValidation;
using HRManagement.API.DTOs.Employee;

namespace HRManagement.API.Validators.Employee
{
    public class UpdateMyProfileValidator
        : AbstractValidator<UpdateMyProfileDto>
    {
        public UpdateMyProfileValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20);
        }
    }

}
