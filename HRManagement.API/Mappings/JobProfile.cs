using AutoMapper;
using HRManagement.API.DTOs;
using HRManagement.API.Models;

namespace HRManagement.API.Mappings
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<Job, JobDTO>();
            CreateMap<JobDTO, Job>();

            CreateMap<Employee, EmployeeDTO>();
        }
    }
}
