using HRManagement.API.Data;
using HRManagement.API.DTOs.Employee;
using HRManagement.API.Exceptions;
using HRManagement.API.Models;
using HRManagement.API.Repository;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly HRContext _context;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            HRContext context)
        {
            _employeeRepository = employeeRepository;
            _context = context;
        }

public async Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            return employees.Select(MapEmployee);
        }

public async Task<EmployeeResponseDto> GetEmployeeByIdAsync(int id,int currentUserId, string currentRole)
        {
            if (currentRole != "Admin" && currentUserId != id)
            {
                throw new ForbiddenException("Access denied");
            }

            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                throw new NotFoundException("Employee not found");
            }

            return MapEmployee(employee);
        }

public async Task<EmployeeResponseDto> CreateEmployeeAsync( CreateEmployeeDto dto)
        {
            var job = await _context.Jobs.FindAsync(dto.JobId);

            if (job == null)
            {
                throw new BadRequestException("Invalid Job ID");
            }

            var department = await _context.Departments.FindAsync(dto.DepartmentId);

            if (department == null)
            {
                throw new BadRequestException("Invalid Department ID");
            }

            if (dto.ManagerId != null)
            {
                bool managerExists =await _employeeRepository.EmployeeExistsAsync(dto.ManagerId.Value);

                if (!managerExists)
                {
                    throw new BadRequestException("Invalid Manager ID");
                }
            }

            decimal salary;

            if (dto.Salary == null)
            {
                salary = job.MinSalary ?? 0;
            }
            else
            {
                salary = dto.Salary.Value;
            }

            if (salary < job.MinSalary || salary > job.MaxSalary)
            {
                throw new ValidationException(
                    $"Salary must be between {job.MinSalary} and {job.MaxSalary}");
            }

            var employee = new Employee
            {
                EmployeeId = await _context.Employees.MaxAsync(e => e.EmployeeId)+ 1,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                JobId = dto.JobId,
                DepartmentId = dto.DepartmentId,
                ManagerId = dto.ManagerId,
                Salary = salary,
                CommissionPct = dto.CommissionPct,
                HireDate = DateOnly.FromDateTime(DateTime.Now),
                Password = null,
                Role = null
            };

            await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            return MapEmployee(employee);
        }

public async Task<string> UpdateEmployeeAsync(int id,UpdateEmployeeDto dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                throw new NotFoundException("Employee not found");
            }

            var job = await _context.Jobs.FindAsync(dto.JobId);

            if (job == null)
            {
                throw new BadRequestException("Invalid Job ID");
            }

            var department =await _context.Departments.FindAsync(dto.DepartmentId);

            if (department == null)
            {
                throw new BadRequestException("Invalid Department ID");
            }

            if (dto.ManagerId != null)
            {
                bool managerExists =await _employeeRepository.EmployeeExistsAsync( dto.ManagerId.Value);

                if (!managerExists)
                {
                    throw new BadRequestException("Invalid Manager ID");
                }
            }

            if (dto.Salary < job.MinSalary || dto.Salary > job.MaxSalary)
            {
                throw new ValidationException($"Salary must be between {job.MinSalary} and {job.MaxSalary}");
            }

bool departmentChanged =employee.DepartmentId !=dto.DepartmentId;

            bool jobChanged =employee.JobId !=dto.JobId;

            if (departmentChanged || jobChanged){
                var lastHistory =await _context.JobHistories.Where(j =>j.EmployeeId ==employee.EmployeeId)
                                                .OrderByDescending(j =>j.EndDate)
                                                .FirstOrDefaultAsync();

                DateOnly startDate;

if (lastHistory == null){
                    startDate =employee.HireDate;
                }

else
                {
                    startDate =lastHistory.EndDate;
                }

                var jobHistory = new JobHistory{
                        EmployeeId =employee.EmployeeId,

                        StartDate =startDate,

                        EndDate =DateOnly.FromDateTime(DateTime.Now),

                        JobId =employee.JobId,

                        DepartmentId =employee.DepartmentId
                };

                await _context.JobHistories.AddAsync(jobHistory);
            }

employee.Email = dto.Email;
            employee.JobId = dto.JobId;
            employee.DepartmentId = dto.DepartmentId;
            employee.ManagerId = dto.ManagerId;
            employee.Salary = dto.Salary;
            employee.CommissionPct = dto.CommissionPct;
            employee.Role = dto.Role;

            _employeeRepository.Update(employee);

            await _employeeRepository.SaveChangesAsync();

            return "Employee updated successfully";
        }

public async Task<string> UpdateRoleAsync( int id,UpdateRoleDto dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                throw new NotFoundException("Employee not found");
            }

            employee.Role = dto.Role;

            _employeeRepository.Update(employee);

            await _employeeRepository.SaveChangesAsync();

            return "Role updated successfully";
        }

public async Task<EmployeeResponseDto> GetMyProfileAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            if (employee == null)
            {
                throw new NotFoundException("Employee not found");
            }

            return MapEmployee(employee);
        }

public async Task<string> UpdateMyProfileAsync(int employeeId,UpdateMyProfileDto dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            if (employee == null)
            {
                throw new NotFoundException("Employee not found");
            }

            employee.Email = dto.Email;
            employee.PhoneNumber = dto.PhoneNumber;

            _employeeRepository.Update(employee);

            await _employeeRepository.SaveChangesAsync();

            return "Profile updated successfully";
        }

public async Task<ManagerResponseDto> GetMyManagerAsync(
            int employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            if (employee == null)
            {
                throw new NotFoundException("Employee not found");
            }

            if (employee.Manager == null)
            {
                throw new NotFoundException("Manager not assigned");
            }

            return new ManagerResponseDto
            {
                EmployeeId = employee.Manager.EmployeeId,
                FullName =employee.Manager.FirstName + " " + employee.Manager.LastName
            };
        }

public async Task<IEnumerable<SubordinateResponseDto>>
            GetMySubordinatesAsync(int employeeId)
        {
            var employees = await _employeeRepository.GetSubordinatesAsync(employeeId);

            return employees.Select(e => new SubordinateResponseDto
            {
                EmployeeId = e.EmployeeId,
                FullName = e.FirstName + " " + e.LastName,
                Email = e.Email,
                JobTitle = e.Job?.JobTitle
            });
        }

public async Task<IEnumerable<EmployeeLookupDto>> GetManagersAsync()
        {
            var managers = await _employeeRepository.GetManagersAsync();

            return managers.Select(e => new EmployeeLookupDto
            {
                EmployeeId = e.EmployeeId,
                FullName = e.FirstName + " " + e.LastName
            });
        }

public async Task<IEnumerable<SearchEmployeeDto>> SearchEmployeesAsync(string name)
        {
            var employees =await _employeeRepository.SearchEmployeesAsync(name);

            return employees.Select(e => new SearchEmployeeDto
            {
                EmployeeId = e.EmployeeId,
                FullName = e.FirstName + " " + e.LastName,
                Email = e.Email,
                DepartmentName = e.Department?.DepartmentName,
                JobTitle = e.Job?.JobTitle
            });
        }

public async Task<IEnumerable<SearchEmployeeDto>> GetEmployeesByDepartmentAsync(short departmentId)
        {
            var employees = await _employeeRepository.GetEmployeesByDepartmentAsync(departmentId);

            return employees.Select(e => new SearchEmployeeDto
            {
                EmployeeId = e.EmployeeId,
                FullName = e.FirstName + " " + e.LastName,
                Email = e.Email,
                DepartmentName = e.Department?.DepartmentName,
                JobTitle = e.Job?.JobTitle
            });
        }

private EmployeeResponseDto MapEmployee(Employee employee)
        {
            decimal totalSalary = employee.Salary ?? 0;

            if (employee.CommissionPct != null)
            {
                totalSalary +=( employee.Salary ?? 0) * (employee.CommissionPct.Value);
            }

            return new EmployeeResponseDto
            {
                EmployeeId = employee.EmployeeId,

                FullName =employee.FirstName + " " + employee.LastName,

                Email = employee.Email,

                JobId = employee.JobId,

                JobTitle = employee.Job?.JobTitle,

                DepartmentId = employee.DepartmentId,

                DepartmentName =employee.Department?.DepartmentName,

                ManagerId = employee.ManagerId,

                ManagerName =
                    employee.Manager != null 
                    ? employee.Manager.FirstName +" " +employee.Manager.LastName : null,

                Salary = employee.Salary,

                CommissionPct = employee.CommissionPct,

                TotalSalary = totalSalary,

                Role = employee.Role,

                HireDate = employee.HireDate
            };
        }
    }
}