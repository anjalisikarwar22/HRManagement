using AutoMapper;
using HRManagement.API.DTOs;
using HRManagement.API.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRManagement.API.Mappings
{
    public class CountryMappingProfile : Profile
    {
        public CountryMappingProfile()
        {
            
            CreateMap<Country, CountryDto>()

                .ForMember(
                    dest => dest.CountryId,
                    opt => opt.MapFrom(
                        src => src.CountryId.Trim()))

                .ForMember(
                    dest => dest.RegionName,
                    opt => opt.MapFrom(
                        src => src.Region != null
                            ? src.Region.RegionName
                            : null));

            CreateMap<CreateCountryDto, Country>()
                
                .ForMember(
                    dest => dest.CountryId,
                    opt => opt.MapFrom(
                        src => src.CountryId != null
                            ? src.CountryId.Trim().ToUpper()
                            : string.Empty));
        }
    }
}