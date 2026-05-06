using HRManagement.API.DTOs;
using HRManagement.API.Exceptions;

namespace HRManagement.API.Validators
{
    public class SalaryValidator
    {
        public void Validate(SalaryDTO dto)
        {
            if (dto.MinSalary < 0)
                throw new ValidationException("MinSalary must be >= 0.");

            if (dto.MaxSalary < 0)
                throw new ValidationException("MaxSalary must be >= 0.");

            if (dto.MaxSalary < dto.MinSalary)
                throw new ValidationException("MaxSalary must be >= MinSalary.");
        }
    }
}
