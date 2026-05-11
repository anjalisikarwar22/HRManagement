using AutoMapper;
using HRManagement.API.DTOs.Location;
using HRManagement.API.Models;

namespace HRManagement.API.Mappings
{
    public class LocationMappingProfile: Profile
    {
        public LocationMappingProfile()
        {

            CreateMap<Location, LocationDto>()
                .ForMember( dest => dest.City,opt => opt.MapFrom(src =>string.IsNullOrWhiteSpace(src.City)? "N/A": src.City))
                .ForMember(dest => dest.StreetAddress,opt => opt.MapFrom(src =>string.IsNullOrWhiteSpace( src.StreetAddress)? "N/A": src.StreetAddress))
                .ForMember(dest => dest.PostalCode,opt => opt.MapFrom(src =>string.IsNullOrWhiteSpace( src.PostalCode)? "N/A": src.PostalCode))
                .ForMember(dest => dest.StateProvince,opt => opt.MapFrom(src =>string.IsNullOrWhiteSpace(src.StateProvince) ? "N/A": src.StateProvince))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src =>string.IsNullOrWhiteSpace(src.CountryId)? "N/A": src.CountryId))
                .ForMember( dest => dest.CountryName, opt => opt.MapFrom(src =>string.IsNullOrWhiteSpace(src.Country!.CountryName)? "N/A": src.Country.CountryName));


            CreateMap<LocationRequestDto,Models.Location>();

            CreateMap<Models.Location,DropdownDto>()
                   .ForMember(dest => dest.Id,opt => opt.MapFrom(src =>src.LocationId))
                   .ForMember( dest => dest.Name, opt => opt.MapFrom(src =>src.City ?? "N/A"));
        }
    }
}