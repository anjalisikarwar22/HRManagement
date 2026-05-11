using AutoMapper;
using HRManagement.API.DTOs;
using HRManagement.API.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRManagement.API.Mappings
{
    public class RegionMappingProfile : Profile
    {
        public RegionMappingProfile()
        {
            CreateMap<Region, RegionDto>();

            CreateMap<CreateRegionDto, Region>();
        }
    }
}