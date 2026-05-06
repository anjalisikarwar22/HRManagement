using HRManagement.API.DTOs;
using HRManagement.API.Exceptions;

namespace HRManagement.API.Validators
{
    public class JobHistoryValidator
    {
        public void Validate(JobHistoryDTO dto)
        {
            if (dto.EmployeeId <= 0)
                throw new ValidationException("EmployeeId is required.");

            if (string.IsNullOrWhiteSpace(dto.JobId))
                throw new ValidationException("JobId is required.");

            if (dto.EndDate < dto.StartDate)
                throw new ValidationException("EndDate must be on or after StartDate.");
        }
    }
}
