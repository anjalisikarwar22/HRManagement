using HRManagement.API.DTOs;
using HRManagement.API.Exceptions;
using HRManagement.API.Models;
using HRManagement.API.Repository;
using HRManagement.API.Services;

namespace HRManagement.API.Services
{
    public class CountryService(
        ICountryRepository countryRepo,
        IRegionRepository regionRepo)
        : ICountryService
    {
        public IEnumerable<CountryDto> GetAllCountries()
            => countryRepo.GetAll()
                .Select(c => new CountryDto
                {
                    CountryId = c.CountryId.Trim(),
                    CountryName = c.CountryName,
                    RegionId = c.RegionId,
                    RegionName = c.Region?.RegionName
                });

        public CountryDto? GetCountryById(string id)
        {
            var country = countryRepo.GetById(id);

            if (country is null) return null;

            return new CountryDto
            {
                CountryId = country.CountryId.Trim(),
                CountryName = country.CountryName,
                RegionId = country.RegionId,
                RegionName = country.Region?.RegionName
            };
        }
        public IEnumerable<CountryDto> GetCountriesByRegion(
            decimal regionId)
            => countryRepo.GetByRegionId(regionId)
                .Select(c => new CountryDto
                {
                    CountryId = c.CountryId.Trim(),
                    CountryName = c.CountryName,
                    RegionId = c.RegionId,
                    RegionName = c.Region?.RegionName
                });

        public void CreateCountry(CreateCountryDto dto)
        {
            var regionExists = regionRepo
                .GetById(dto.RegionId);

            if (regionExists is null)
                throw new ValidationException(
                    $"Region with id {dto.RegionId}" +
                    $" does not exist");

            var upperCode = dto.CountryId?
                .Trim().ToUpper() ?? string.Empty;

            var countryExists = countryRepo
                .GetById(upperCode);

            if (countryExists is not null)
                throw new DuplicateException(
                    $"Country '{upperCode}' already exists");

            var country = new Country
            {
                CountryId = upperCode,
                CountryName = dto.CountryName,
                RegionId = dto.RegionId
            };

            countryRepo.Add(country);
            countryRepo.SaveChanges();
        }

        public void UpdateCountry(
            string id, CreateCountryDto dto)
        {
            var country = countryRepo.GetById(id);

            if (country is null)
                throw new NotFoundException(
                    $"Country '{id}' not found");

            var regionExists = regionRepo
                .GetById(dto.RegionId);

            if (regionExists is null)
                throw new ValidationException(
                    $"Region with id {dto.RegionId}" +
                    $" does not exist");

            country.CountryName = dto.CountryName;
            country.RegionId = dto.RegionId;

            countryRepo.Update(country);
            countryRepo.SaveChanges();
        }

        public IEnumerable<RegionDto> GetRegionsForDropdown()
            => regionRepo.GetAll()
                .Select(r => new RegionDto
                {
                    RegionId = r.RegionId,
                    RegionName = r.RegionName
                });
    }
}
