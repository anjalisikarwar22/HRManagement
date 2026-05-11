using AutoMapper;
using HRManagement.API.DTOs.Employee;
using HRManagement.API.Models;

namespace HRManagement.API.Mappings
{
    public class EmployeeMappingProfile
        : Profile
    {
        public EmployeeMappingProfile()
        {
            
            CreateMap<Employee,EmployeeResponseDto>()
                .ForMember(dest => dest.FullName,opt => opt.MapFrom(src =>src.FirstName+ " " + src.LastName))
                .ForMember(dest => dest.JobTitle,opt => opt.MapFrom(src =>src.Job != null ? src.Job.JobTitle: null))
                .ForMember(dest => dest.DepartmentName,opt => opt.MapFrom(src =>src.Department != null? src.Department.DepartmentName: null))
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src =>src.Manager != null ? src.Manager.FirstName + " "+ src.Manager.LastName : null))
                .ForMember(dest => dest.TotalSalary,opt => opt.MapFrom(src =>(src.Salary ?? 0)+(src.CommissionPct != null ? (src.Salary ?? 0) * src.CommissionPct.Value :0)));


            CreateMap<CreateEmployeeDto,Employee>();

            CreateMap< UpdateEmployeeDto,Employee>();
            
            CreateMap<UpdateMyProfileDto, Employee>();



            CreateMap<Employee,EmployeeSummaryDto>()
                .ForMember(dest => dest.FullName,opt => opt.MapFrom(src =>src.FirstName+ " " + src.LastName))
                .ForMember(dest => dest.JobTitle,opt => opt.MapFrom(src =>src.Job != null? src.Job.JobTitle: null))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src =>src.Department != null ? src.Department.DepartmentName : null));



            CreateMap<Employee,EmployeeLookupDto>()
                .ForMember(dest => dest.FullName,opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
        }
    }
}
