using HRManagement.API.DTOs.Departments;
using FluentValidation;
using AppValidationException = HRManagement.API.Exceptions.ValidationException;

namespace HRManagement.API.Validators
{
    public class DepartmentValidator
    {
        private const decimal MaxDepartmentId = 9999;
        private const decimal MaxManagerId = 999999;
        private const decimal MaxLocationId = 9999;
        private readonly IValidator<CreateDepartmentDto> _createValidator;
        private readonly IValidator<UpdateDepartmentDto> _updateValidator;

        public DepartmentValidator()
            : this(new CreateDepartmentFluentValidator(), new UpdateDepartmentFluentValidator())
        {
        }

        public DepartmentValidator(
            IValidator<CreateDepartmentDto> createValidator,
            IValidator<UpdateDepartmentDto> updateValidator)
        {
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public void ValidateDepartmentId(decimal departmentId)
        {
            ValidateRequiredId(departmentId, "DepartmentId", MaxDepartmentId);
        }

        public void ValidateManagerId(decimal managerId)
        {
            ValidateRequiredId(managerId, "ManagerId", MaxManagerId);
        }

        public void ValidateLocationId(decimal locationId)
        {
            ValidateRequiredId(locationId, "LocationId", MaxLocationId);
        }

        public void ValidateCreate(CreateDepartmentDto dto)
        {
            ValidateFluent(_createValidator.Validate(dto));
        }

        public void ValidateUpdate(UpdateDepartmentDto dto)
        {
            ValidateFluent(_updateValidator.Validate(dto));
        }

        public void ValidateSearch(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new AppValidationException("Search name is required.");

            if (name.Length > 30)
                throw new AppValidationException("Search name must be 30 characters or fewer.");
        }

        public void ValidatePaging(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new AppValidationException("PageNumber must be greater than 0.");

            if (pageSize < 1)
                throw new AppValidationException("PageSize must be greater than 0.");

            if (pageSize > 100)
                throw new AppValidationException("PageSize cannot be greater than 100.");
        }

        private static void ValidateFluent(FluentValidation.Results.ValidationResult result)
        {
            if (!result.IsValid)
                throw new AppValidationException(result.Errors[0].ErrorMessage);
        }

        private static void ValidateRequiredId(decimal id, string fieldName, decimal maxValue)
        {
            if (id <= 0)
                throw new AppValidationException($"{fieldName} must be greater than 0.");

            if (id != decimal.Truncate(id))
                throw new AppValidationException($"{fieldName} must be a whole number.");

            if (id > maxValue)
                throw new AppValidationException($"{fieldName} is too large.");
        }
    }
}
