using FluentValidation;
using HRManagement.API.DTOs.Employee;

namespace HRManagement.API.Validators.Employee
{
    public class UpdateRoleValidator
        : AbstractValidator<UpdateRoleDto>
    {
        public UpdateRoleValidator()
        {
            RuleFor(x => x.Role)
                .NotEmpty()
                .Must(role =>
                    role == "Admin" ||
                    role == "Employee")
                .WithMessage(
                    "Role must be Admin or Employee");
        }
    }
}
