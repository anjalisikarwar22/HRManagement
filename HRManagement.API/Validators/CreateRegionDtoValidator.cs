using FluentValidation;
using HRManagement.API.DTOs;
namespace HRManagement.API.Validators
{
        public class CreateRegionDtoValidator : AbstractValidator<CreateRegionDto>
        {
            public CreateRegionDtoValidator()
            {
                RuleFor(x => x.RegionName).NotEmpty().WithMessage("Region name is required.");

                RuleFor(x => x.RegionName).MinimumLength(3).WithMessage("Region name must have at least 3 characters");

                RuleFor(x => x.RegionName).MaximumLength(25).WithMessage("Region name must not exceed 25 characters.");

                RuleFor(x=>x.RegionName).Matches(@"^[a-zA-Z\s]+$").WithMessage("Region name must only contain letters and spaces.");
            }
        }
    }
