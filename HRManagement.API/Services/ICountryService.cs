using HRManagement.API.DTOs;
namespace HRManagement.API.Services.Interfaces
{
    public interface ICountryService
    {
        IEnumerable<CountryDto> GetAllCountries();
        CountryDto GetCountryById(string id);
        IEnumerable<CountryDto> GetCountriesByRegion(decimal regionId);
        void CreateCountry(CreateCountryDto dto);
        void UpdateCountry(string id, CreateCountryDto dto);

        IEnumerable<RegionDto> GetRegionsForDropdown();
    }
}
