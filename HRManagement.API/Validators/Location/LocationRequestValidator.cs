using FluentValidation;
using HRManagement.API.DTOs.Location;

namespace HRManagement.API.Validators.Location
{
    public class LocationRequestValidator : AbstractValidator<LocationRequestDto>
    {
        public LocationRequestValidator()
        {
            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("City is required")
                .MaximumLength(30)
                .WithMessage("City cannot exceed 30 characters");


            RuleFor(x => x.StreetAddress)
                .NotEmpty()
                .MaximumLength(40)
                .WithMessage("Street Address cannot exceed 40 characters");


            RuleFor(x => x.PostalCode)
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
                .WithMessage("Country Id cannot exceed 2 characters");


                //.MustAsync(async (countryId, cancellation) =>
                //    await countryRepository.ExistsAsync(countryId))
        }
    }
}