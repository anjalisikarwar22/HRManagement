using HRManagement.API.DTOs;
using HRManagement.API.Exceptions;

namespace HRManagement.API.Validators
{
    public class JobValidator
    {
        public void Validate(JobDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.JobId))
                throw new ValidationException("JobId is required.");

            if (dto.JobId.Length > 10)
                throw new ValidationException("JobId must be 10 characters or fewer.");

            if (string.IsNullOrWhiteSpace(dto.JobTitle))
                throw new ValidationException("JobTitle is required.");

            if (dto.JobTitle.Length > 35)
                throw new ValidationException("JobTitle must be 35 characters or fewer.");

            if (dto.MinSalary < 0)
                throw new ValidationException("MinSalary must be >= 0.");

            if (dto.MaxSalary < dto.MinSalary)
                throw new ValidationException("MaxSalary must be >= MinSalary.");
        }
    }
}
