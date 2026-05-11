using AutoMapper;
using HRManagement.API.DTOs;
using HRManagement.API.Models;

namespace HRManagement.API.Mappings
{
    public class JobHistoryProfile : Profile
    {
        public JobHistoryProfile()
        {
            CreateMap<JobHistory, JobHistoryDTO>();
            CreateMap<JobHistoryDTO, JobHistory>();
        }
    }
}
