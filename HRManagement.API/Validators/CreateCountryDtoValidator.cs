using FluentValidation;
using HRManagement.API.DTOs;
namespace HRManagement.API.Validators
{
    public class CreateCountryDtoValidator : AbstractValidator<CreateCountryDto>
    {
        public CreateCountryDtoValidator()
        {
            RuleFor(x => x.CountryId).NotEmpty().WithMessage("Country ID is required.");

            RuleFor(x => x.CountryId).Length(2,2).WithMessage("Country ID must be exactly 2 characters.");

            RuleFor(x => x.CountryId).Matches(@"^[a-zA-Z]+$").WithMessage("Country code must contain letters only");

            RuleFor(x => x.CountryName).NotEmpty().WithMessage("Country name is required");

            RuleFor(x => x.CountryName).MinimumLength(3).WithMessage("Country name must be at least 3 characters");

            RuleFor(x => x.CountryName).MaximumLength(60).WithMessage("Country name cannot exceed 60 characters");

            RuleFor(x => x.CountryName).Matches(@"^[a-zA-Z\s]+$").WithMessage("Country name must contain letters and spaces only");

            RuleFor(x => x.RegionId).NotEmpty().WithMessage("Region ID is required");

            RuleFor(x => x.RegionId).GreaterThan(0).WithMessage("Region ID must be a positive number greater than 0");
        }
    }
}
