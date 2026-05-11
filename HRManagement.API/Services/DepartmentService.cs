using HRManagement.API.DTOs.Departments;
using HRManagement.API.Interfaces;
using HRManagement.API.Models;
using HRManagement.API.Validators;

namespace HRManagement.API.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly DepartmentValidator _validator;

        public DepartmentService(
            IDepartmentRepository departmentRepository,
            DepartmentValidator validator)
        {
            _departmentRepository = departmentRepository;
            _validator = validator;
        }

        public async Task<IEnumerable<DepartmentListDto>> GetAllAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();
            return departments.Select(ToListDto);
        }

        public async Task<DepartmentDto?> GetByIdAsync(decimal departmentId)
        {
            _validator.ValidateDepartmentId(departmentId);

            var department = await _departmentRepository.GetByIdAsync(departmentId);
            return department == null ? null : ToDto(department);
        }

        public async Task<IEnumerable<DepartmentListDto>> GetByLocationAsync(decimal locationId)
        {
            _validator.ValidateLocationId(locationId);

            var departments = await _departmentRepository.GetByLocationAsync(locationId);
            return departments.Select(ToListDto);
        }

        public async Task<IEnumerable<DepartmentListDto>> GetByManagerAsync(decimal managerId)
        {
            _validator.ValidateManagerId(managerId);

            var departments = await _departmentRepository.GetByManagerAsync(managerId);
            return departments.Select(ToListDto);
        }

        public async Task<IEnumerable<DepartmentListDto>> SearchByNameAsync(string name)
        {
            _validator.ValidateSearch(name);

            var departments = await _departmentRepository.SearchByNameAsync(name);
            return departments.Select(ToListDto);
        }

        public async Task<PagedDepartmentDto> GetPagedAsync(int pageNumber, int pageSize)
        {
            _validator.ValidatePaging(pageNumber, pageSize);

            var (departments, totalCount) =
                await _departmentRepository.GetPagedAsync(pageNumber, pageSize);

            return new PagedDepartmentDto
            {
                Items = departments.Select(ToListDto),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<DepartmentSummaryDto> GetSummaryAsync()
        {
            return await _departmentRepository.GetSummaryAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _departmentRepository.GetCountAsync();
        }

        public async Task<IEnumerable<DepartmentDropdownDto>> GetDropdownAsync()
        {
            var departments = await _departmentRepository.GetDropdownAsync();
            return departments.Select(ToDropdownDto);
        }

        public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
        {
            _validator.ValidateCreate(dto);

            var department = new Department
            {
                DepartmentName = dto.DepartmentName.Trim(),
                ManagerId = dto.ManagerId,
                LocationId = dto.LocationId
            };

            var created = await _departmentRepository.CreateAsync(department);
            return ToDto(created);
        }

        public async Task<DepartmentDto?> UpdateAsync(
            decimal departmentId,
            UpdateDepartmentDto dto)
        {
            _validator.ValidateDepartmentId(departmentId);
            _validator.ValidateUpdate(dto);

            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
                return null;

            department.DepartmentName = dto.DepartmentName.Trim();
            department.ManagerId = dto.ManagerId;
            department.LocationId = dto.LocationId;

            await _departmentRepository.UpdateAsync(department);
            return ToDto(department);
        }

        private static DepartmentDto ToDto(Department department)
        {
            return new DepartmentDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                ManagerId = department.ManagerId,
                LocationId = department.LocationId
            };
        }

        private static DepartmentListDto ToListDto(Department department)
        {
            return new DepartmentListDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                ManagerId = department.ManagerId,
                LocationId = department.LocationId
            };
        }

        private static DepartmentDropdownDto ToDropdownDto(Department department)
        {
            return new DepartmentDropdownDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName
            };
        }
    }
}
