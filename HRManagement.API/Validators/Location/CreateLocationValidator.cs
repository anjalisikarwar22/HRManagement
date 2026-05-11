using FluentValidation;
using HRManagement.API.DTOs.Location;

namespace HRManagement.API.Validators.Location
{
    public class CreateLocationValidator : AbstractValidator<CreateLocationDto>
    {
        public CreateLocationValidator()
        {
            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("City is required")
                .MaximumLength(30)
                .WithMessage("City cannot exceed 30 characters");

<<<<<<< HEAD
            RuleFor(x => x.StreetAddress)
=======
RuleFor(x => x.StreetAddress)
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
                .NotEmpty()
                .MaximumLength(40)
                .WithMessage("Street Address cannot exceed 40 characters");

<<<<<<< HEAD
            RuleFor(x => x.PostalCode)
=======
RuleFor(x => x.PostalCode)
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
                .MaximumLength(12)
                .WithMessage("Postal Code cannot exceed 12 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.PostalCode));

            RuleFor(x => x.StateProvince)
                .MaximumLength(25)
                .WithMessage("State Province cannot exceed 25 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.StateProvince));

            RuleFor(x => x.CountryId)
                .NotEmpty()
                .MaximumLength(2)
                .Matches("^[A-Z]{2}$")
<<<<<<< HEAD
                .WithMessage("Country Id must be 2 uppercase characters");
        }
=======
                .WithMessage("Country Id cannot exceed 2 characters");

}
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
    }
}
