using FluentValidation;
using HRManagement.API.DTOs.Location;

namespace HRManagement.API.Validators.Location
{
    public class CreateLocationValidator : AbstractValidator<CreateLocationDto>
    {
        public CreateLocationValidator()
        {
            // Location Id Validation
            RuleFor(x => x.LocationId)
                .NotEmpty()
                .WithMessage("Location Id is required")

                .InclusiveBetween(1000, 9999)
                .WithMessage("Location Id must be a 4 digit number")

                .Must(id => id % 100 == 0)
                .WithMessage("Location Id must be like 1100, 1200, 1300 etc.");

            // City Validation
            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("City is required")
                .MaximumLength(30)
                .WithMessage("City cannot exceed 30 characters");


            // Street Address Validation
            RuleFor(x => x.StreetAddress)
                .NotEmpty()
                .MaximumLength(40)
                .WithMessage("Street Address cannot exceed 40 characters");


            // Postal Code Validation
            RuleFor(x => x.PostalCode)
                .MaximumLength(12)
                .WithMessage("Postal Code cannot exceed 12 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.PostalCode));

            // State Province Validation
            RuleFor(x => x.StateProvince)
                .MaximumLength(25)
                .WithMessage("State Province cannot exceed 25 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.StateProvince));

            // Country Id Validation
            RuleFor(x => x.CountryId)
                .NotEmpty()
                .MaximumLength(2)
                .Matches("^[A-Z]{2}$")
                .WithMessage("Country Id cannot exceed 2 characters");


                //.MustAsync(async (countryId, cancellation) =>
                //    await countryRepository.ExistsAsync(countryId))
        }
    }
}