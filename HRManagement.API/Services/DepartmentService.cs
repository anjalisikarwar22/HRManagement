using AutoMapper;
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
        private readonly IMapper _mapper;

        public DepartmentService(
            IDepartmentRepository departmentRepository,
            DepartmentValidator validator,
            IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentListDto>> GetAllAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DepartmentListDto>>(departments);
        }

        public async Task<DepartmentDto?> GetByIdAsync(decimal departmentId)
        {
            _validator.ValidateDepartmentId(departmentId);

            var department = await _departmentRepository.GetByIdAsync(departmentId);
            return department == null ? null : _mapper.Map<DepartmentDto>(department);
        }

        public async Task<IEnumerable<DepartmentListDto>> GetByLocationAsync(decimal locationId)
        {
            _validator.ValidateLocationId(locationId);

            var departments = await _departmentRepository.GetByLocationAsync(locationId);
            return _mapper.Map<IEnumerable<DepartmentListDto>>(departments);
        }

        public async Task<IEnumerable<DepartmentListDto>> GetByManagerAsync(decimal managerId)
        {
            _validator.ValidateManagerId(managerId);

            var departments = await _departmentRepository.GetByManagerAsync(managerId);
            return _mapper.Map<IEnumerable<DepartmentListDto>>(departments);
        }

        public async Task<IEnumerable<DepartmentListDto>> SearchByNameAsync(string name)
        {
            _validator.ValidateSearch(name);

            var departments = await _departmentRepository.SearchByNameAsync(name);
            return _mapper.Map<IEnumerable<DepartmentListDto>>(departments);
        }

        public async Task<PagedDepartmentDto> GetPagedAsync(int pageNumber, int pageSize)
        {
            _validator.ValidatePaging(pageNumber, pageSize);

            var (departments, totalCount) =
                await _departmentRepository.GetPagedAsync(pageNumber, pageSize);

            return new PagedDepartmentDto
            {
                Items = _mapper.Map<IEnumerable<DepartmentListDto>>(departments),
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
            return _mapper.Map<IEnumerable<DepartmentDropdownDto>>(departments);
        }

        public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
        {
            _validator.ValidateCreate(dto);

            var department = _mapper.Map<Department>(dto);
            var created = await _departmentRepository.CreateAsync(department);
            return _mapper.Map<DepartmentDto>(created);
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

            _mapper.Map(dto, department);
            await _departmentRepository.UpdateAsync(department);
            return _mapper.Map<DepartmentDto>(department);
        }
    }
}
