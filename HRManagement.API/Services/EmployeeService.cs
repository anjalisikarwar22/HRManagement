using AutoMapper;
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
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository,HRContext context, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _context = context;
            _mapper = mapper;

        }


        public async Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);
        }

       

        public async Task<EmployeeResponseDto> GetEmployeeByIdAsync(decimal id,decimal currentUserId, string currentRole)
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
                throw new ValidationException($"Salary must be between {job.MinSalary} and {job.MaxSalary}");
            }

            var employee = _mapper.Map<Employee>(dto);


            employee.EmployeeId= await _employeeRepository.GetMaxEmployeeIdAsync() + 1;

            employee.Salary = salary;

            employee.HireDate =DateOnly.FromDateTime( DateTime.Now);

            employee.Password = null;

            employee.Role = null;

            await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            return _mapper.Map<EmployeeResponseDto>(employee);
        }

        
        public async Task<string> UpdateEmployeeAsync(decimal id,UpdateEmployeeDto dto)
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

            _mapper.Map(dto, employee);

            _employeeRepository.Update(employee);

            await _employeeRepository.SaveChangesAsync();

            return "Employee updated successfully";
        }

        

        public async Task<EmployeeResponseDto> GetMyProfileAsync(decimal employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            if (employee == null)
            {
                throw new NotFoundException("Employee not found");
            }

            return _mapper.Map<EmployeeResponseDto>(employee);
        }

        
        public async Task<string> UpdateMyProfileAsync(decimal employeeId,UpdateMyProfileDto dto)
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

        

        public async Task<EmployeeLookupDto> GetMyManagerAsync(decimal employeeId)
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

            return _mapper.Map< EmployeeLookupDto>(employee.Manager);
        }

        

        public async Task<IEnumerable<EmployeeSummaryDto>> GetMySubordinatesAsync(decimal employeeId)
        {
            var employees = await _employeeRepository.GetSubordinatesAsync(employeeId);

            return _mapper.Map< IEnumerable<EmployeeSummaryDto>>(employees);
        }

        public async Task<IEnumerable<EmployeeLookupDto>> GetManagersAsync()
        {
            var managers = await _employeeRepository.GetManagersAsync();

            return _mapper.Map<IEnumerable<EmployeeLookupDto>>(managers);
        }

        

        public async Task<IEnumerable<EmployeeSummaryDto>> SearchEmployeesAsync(string name)
        {
            var employees =await _employeeRepository.SearchEmployeesAsync(name);

            return _mapper.Map<IEnumerable<EmployeeSummaryDto>>(employees);
        }

        
        

        public async Task<IEnumerable<EmployeeSummaryDto>> GetEmployeesByDepartmentAsync(short departmentId)
        {
            var employees = await _employeeRepository.GetEmployeesByDepartmentAsync(departmentId);

            return _mapper.Map<IEnumerable<EmployeeSummaryDto>>(employees);
        }

        

       
    }
}