using AutoMapper;
using HRManagement.API.DTOs.Employee;
using HRManagement.API.Exceptions;
using HRManagement.API.Interfaces;
using HRManagement.API.Models;
using HRManagement.API.Repository;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

<<<<<<< Updated upstream
        private readonly IJobHistoryRepository _jobHistoryRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, IJobHistoryRepository jobHistoryRepository,
            IJobRepository jobRepository, IDepartmentRepository departmentRepository)
=======
        public EmployeeService(IEmployeeRepository employeeRepository, HRContext context)
>>>>>>> Stashed changes
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _jobHistoryRepository = jobHistoryRepository;
            _jobRepository = jobRepository;
            _departmentRepository = departmentRepository;

        }

<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
        public async Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);
        }

<<<<<<< Updated upstream


        public async Task<EmployeeResponseDto> GetEmployeeByIdAsync(decimal id, decimal currentUserId, string currentRole)
=======
        public async Task<EmployeeResponseDto> GetEmployeeByIdAsync(int id, int currentUserId, string currentRole)
>>>>>>> Stashed changes
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

            return _mapper.Map<EmployeeResponseDto>(employee);
        }

<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
        public async Task<EmployeeResponseDto> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            var job = await _jobRepository.GetById(dto.JobId);


            if (job == null)
            {
                throw new BadRequestException("Invalid Job ID");
            }

            var department = await _departmentRepository.GetByIdAsync((decimal)dto.DepartmentId);

            if (department == null)
            {
                throw new BadRequestException("Invalid Department ID");
            }

            if (dto.ManagerId != null)
            {
                bool managerExists = await _employeeRepository.EmployeeExistsAsync(dto.ManagerId.Value);

                if (!managerExists)
                {
                    throw new BadRequestException("Invalid Manager ID");
                }
            }

            decimal salary = dto.Salary ?? job.MinSalary ?? 0;

            if (salary < job.MinSalary || salary > job.MaxSalary)
            {
                throw new ValidationException($"Salary must be between {job.MinSalary} and {job.MaxSalary}");
            }

<<<<<<< Updated upstream
            var employee = _mapper.Map<Employee>(dto);


            employee.EmployeeId = await _employeeRepository.GetMaxEmployeeIdAsync() + 1;

            employee.Salary = salary;

            employee.HireDate = DateOnly.FromDateTime(DateTime.Now);

            employee.Password = null;

            employee.Role = null;
=======
            var employee = new Employee
            {
                EmployeeId = await _context.Employees.MaxAsync(e => e.EmployeeId) + 1,
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
>>>>>>> Stashed changes

            await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            return _mapper.Map<EmployeeResponseDto>(employee);
        }

<<<<<<< Updated upstream

        public async Task<string> UpdateEmployeeAsync(decimal id, UpdateEmployeeDto dto)
=======
        public async Task<string> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto)
>>>>>>> Stashed changes
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                throw new NotFoundException("Employee not found");
            }

            var job = await _jobRepository.GetById(dto.JobId);

            if (job == null)
            {
                throw new BadRequestException("Invalid Job ID");
            }

<<<<<<< Updated upstream
            var department = await _departmentRepository.GetByIdAsync((decimal)dto.DepartmentId);
=======
            var department = await _context.Departments.FindAsync(dto.DepartmentId);
>>>>>>> Stashed changes

            if (department == null)
            {
                throw new BadRequestException("Invalid Department ID");
            }

            if (dto.ManagerId != null)
            {
                bool managerExists = await _employeeRepository.EmployeeExistsAsync(dto.ManagerId.Value);

                if (!managerExists)
                {
                    throw new BadRequestException("Invalid Manager ID");
                }
            }

            if (dto.Salary < job.MinSalary || dto.Salary > job.MaxSalary)
            {
                throw new ValidationException($"Salary must be between {job.MinSalary} and {job.MaxSalary}");
            }

<<<<<<< Updated upstream

            bool departmentChanged = employee.DepartmentId != dto.DepartmentId;

            bool jobChanged = employee.JobId != dto.JobId;

            if (departmentChanged || jobChanged)
            {
                var lastHistory =await _jobHistoryRepository.GetLatestHistoryAsync(employee.EmployeeId);

                DateOnly startDate;


                if (lastHistory == null)
                {
                    startDate = employee.HireDate;
                }

                else
                {
=======
            bool departmentChanged = employee.DepartmentId != dto.DepartmentId;
            bool jobChanged = employee.JobId != dto.JobId;

            if (departmentChanged || jobChanged)
            {
                var lastHistory = await _context.JobHistories
                    .Where(j => j.EmployeeId == employee.EmployeeId)
                    .OrderByDescending(j => j.EndDate)
                    .FirstOrDefaultAsync();

                DateOnly startDate;

                if (lastHistory == null)
                {
                    startDate = employee.HireDate;
                }
                else
                {
>>>>>>> Stashed changes
                    startDate = lastHistory.EndDate;
                }

                var jobHistory = new JobHistory
                {
                    EmployeeId = employee.EmployeeId,
<<<<<<< Updated upstream

                    StartDate = startDate,

                    EndDate = DateOnly.FromDateTime(DateTime.Now),

                    JobId = employee.JobId,

=======
                    StartDate = startDate,
                    EndDate = DateOnly.FromDateTime(DateTime.Now),
                    JobId = employee.JobId,
>>>>>>> Stashed changes
                    DepartmentId = employee.DepartmentId
                };

                await _jobHistoryRepository.Add(jobHistory);
            }

<<<<<<< Updated upstream
            _mapper.Map(dto, employee);
=======
            employee.Email = dto.Email;
            employee.JobId = dto.JobId;
            employee.DepartmentId = dto.DepartmentId;
            employee.ManagerId = dto.ManagerId;
            employee.Salary = dto.Salary;
            employee.CommissionPct = dto.CommissionPct;
            employee.Role = dto.Role;
>>>>>>> Stashed changes

            _employeeRepository.Update(employee);
            await _employeeRepository.SaveChangesAsync();

            return "Employee updated successfully";
        }

<<<<<<< Updated upstream
=======
        public async Task<string> UpdateRoleAsync(int id, UpdateRoleDto dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
>>>>>>> Stashed changes


<<<<<<< Updated upstream
        public async Task<EmployeeResponseDto> GetMyProfileAsync(decimal employeeId)
=======
            employee.Role = dto.Role;

            _employeeRepository.Update(employee);
            await _employeeRepository.SaveChangesAsync();

            return "Role updated successfully";
        }

        public async Task<EmployeeResponseDto> GetMyProfileAsync(int employeeId)
>>>>>>> Stashed changes
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            if (employee == null)
            {
                throw new NotFoundException("Employee not found");
            }

            return _mapper.Map<EmployeeResponseDto>(employee);
        }

<<<<<<< Updated upstream

        public async Task<string> UpdateMyProfileAsync(decimal employeeId, UpdateMyProfileDto dto)
=======
        public async Task<string> UpdateMyProfileAsync(int employeeId, UpdateMyProfileDto dto)
>>>>>>> Stashed changes
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            if (employee == null)
            {
                throw new NotFoundException("Employee not found");
            }

            _mapper.Map(dto, employee);

            _employeeRepository.Update(employee);
            await _employeeRepository.SaveChangesAsync();

            return "Profile updated successfully";
        }

<<<<<<< Updated upstream


        public async Task<EmployeeLookupDto> GetMyManagerAsync(decimal employeeId)
=======
        public async Task<ManagerResponseDto> GetMyManagerAsync(int employeeId)
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
            return _mapper.Map<EmployeeLookupDto>(employee.Manager);
        }



        public async Task<IEnumerable<EmployeeSummaryDto>> GetMySubordinatesAsync(decimal employeeId)
=======
            return new ManagerResponseDto
            {
                EmployeeId = employee.Manager.EmployeeId,
                FullName = employee.Manager.FirstName + " " + employee.Manager.LastName
            };
        }

        public async Task<IEnumerable<SubordinateResponseDto>> GetMySubordinatesAsync(int employeeId)
>>>>>>> Stashed changes
        {
            var employees = await _employeeRepository.GetSubordinatesAsync(employeeId);

            return _mapper.Map<IEnumerable<EmployeeSummaryDto>>(employees);
        }

        public async Task<IEnumerable<EmployeeLookupDto>> GetManagersAsync()
        {
            var managers = await _employeeRepository.GetManagersAsync();

            return _mapper.Map<IEnumerable<EmployeeLookupDto>>(managers);
        }

<<<<<<< Updated upstream


        public async Task<IEnumerable<EmployeeSummaryDto>> SearchEmployeesAsync(string name)
=======
        public async Task<IEnumerable<SearchEmployeeDto>> SearchEmployeesAsync(string name)
>>>>>>> Stashed changes
        {
            var employees = await _employeeRepository.SearchEmployeesAsync(name);

            return _mapper.Map<IEnumerable<EmployeeSummaryDto>>(employees);
        }

<<<<<<< Updated upstream



        public async Task<IEnumerable<EmployeeSummaryDto>> GetEmployeesByDepartmentAsync(short departmentId)
=======
        public async Task<IEnumerable<SearchEmployeeDto>> GetEmployeesByDepartmentAsync(short departmentId)
>>>>>>> Stashed changes
        {
            var employees = await _employeeRepository.GetEmployeesByDepartmentAsync(departmentId);

            return _mapper.Map<IEnumerable<EmployeeSummaryDto>>(employees);
        }

<<<<<<< Updated upstream



=======
        private EmployeeResponseDto MapEmployee(Employee employee)
        {
            decimal totalSalary = employee.Salary ?? 0;

            if (employee.CommissionPct != null)
            {
                totalSalary += (employee.Salary ?? 0) * employee.CommissionPct.Value;
            }

            return new EmployeeResponseDto
            {
                EmployeeId = employee.EmployeeId,
                FullName = employee.FirstName + " " + employee.LastName,
                Email = employee.Email,
                JobId = employee.JobId,
                JobTitle = employee.Job?.JobTitle,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.DepartmentName,
                ManagerId = employee.ManagerId,
                ManagerName = employee.Manager != null
                    ? employee.Manager.FirstName + " " + employee.Manager.LastName
                    : null,
                Salary = employee.Salary,
                CommissionPct = employee.CommissionPct,
                TotalSalary = totalSalary,
                Role = employee.Role,
                HireDate = employee.HireDate
            };
        }
>>>>>>> Stashed changes
    }
}