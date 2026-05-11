using AutoMapper;
using HRManagement.API.DTOs.Departments;
using HRManagement.API.Models;

namespace HRManagement.API.Mappings
{
    public class DepartmentMappingProfile : Profile
    {
        public DepartmentMappingProfile()
        {
            CreateMap<Department, DepartmentDto>();
            CreateMap<Department, DepartmentListDto>();
            CreateMap<Department, DepartmentDropdownDto>();
            CreateMap<CreateDepartmentDto, Department>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.DepartmentName.Trim()));
            CreateMap<UpdateDepartmentDto, Department>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.DepartmentName.Trim()))
                .ForMember(dest => dest.DepartmentId, opt => opt.Ignore())
                .ForMember(dest => dest.Employees, opt => opt.Ignore())
                .ForMember(dest => dest.JobHistories, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore())
                .ForMember(dest => dest.Manager, opt => opt.Ignore());
        }
    }
}
