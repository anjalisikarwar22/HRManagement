using HRManagement.API.DTOs;
using HRManagement.API.Models;  
using HRManagement.API.Repositories.Interfaces; 
using HRManagement.API.Services.Interfaces;
namespace HRManagement.API.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepo;
        private readonly IRegionRepository _regionRepo;
        public CountryService(ICountryRepository countryRepo, IRegionRepository regionRepo)
        {
            _countryRepo = countryRepo;
            _regionRepo = regionRepo;
        }
        public IEnumerable<CountryDto> GetAllCountries()
        {
            var countries = _countryRepo.GetAll();
            return countries.Select(c => new CountryDto
            {
                CountryId = c.CountryId.Trim(),
                CountryName = c.CountryName,
                RegionId = c.RegionId,
                RegionName = c.Region?.RegionName
            });
        }
        public CountryDto GetCountryById(string id)
        {
            var country = _countryRepo.GetById(id);
            if (country == null)
            {
                return null;
            }
            return new CountryDto
            {
                CountryId = country.CountryId.Trim(),
                CountryName = country.CountryName,
                RegionId = country.RegionId,
                RegionName = country.Region?.RegionName
            };
        }
        public IEnumerable<CountryDto> GetCountriesByRegion(decimal regionId)
        {
            var countries = _countryRepo.GetByRegionId(regionId);
            return countries.Select(c => new CountryDto
            {
                CountryId = c.CountryId.Trim(),
                CountryName = c.CountryName,
                RegionId = c.RegionId,
                RegionName = c.Region?.RegionName
            });
        }
        public void CreateCountry(CreateCountryDto dto)
        {
            var regionexists = _regionRepo.GetById(dto.RegionId);
            if (regionexists == null)
            {
                throw new Exception($"Region with id '{dto.RegionId}' does not exist");
            }
            var countryexists = _countryRepo.GetById(dto.CountryId.Trim().ToUpper());
            if (countryexists != null)
            {
                throw new Exception($"Country with code '{dto.CountryId.ToUpper()}' already exists");
            }
            dto.CountryId = dto.CountryId.Trim().ToUpper();
            var country = new Country
            {
                CountryId = dto.CountryId,
                CountryName = dto.CountryName,
                RegionId = dto.RegionId
            };
            _countryRepo.Add(country);
            _countryRepo.SaveChanges();
        }
        public void UpdateCountry(string id, CreateCountryDto dto)
        {
            var country = _countryRepo.GetById(id);
            if (country == null)
            {
                throw new Exception($"Country with code '{id}' not found");
            }

            var regionExists = _regionRepo.GetById(dto.RegionId);
            if (regionExists == null)
            {
                throw new Exception($"Region with id {dto.RegionId} does not exist");
            }

            country.CountryName = dto.CountryName;
            country.RegionId = dto.RegionId;


            _countryRepo.Update(country);
            _countryRepo.SaveChanges();
        }


        public IEnumerable<RegionDto> GetRegionsForDropdown()
        {
            return _regionRepo.GetAll()
                .Select(r => new RegionDto
                {
                    RegionId = r.RegionId,
                    RegionName = r.RegionName
                });
        }
    }
}
