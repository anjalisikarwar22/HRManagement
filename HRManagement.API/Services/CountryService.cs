using AutoMapper;
using HRManagement.API.DTOs;
using HRManagement.API.Exceptions;
using HRManagement.API.Models;
using HRManagement.API.Repository;

namespace HRManagement.API.Services
{
    public class CountryService(
        ICountryRepository countryRepo,
        IRegionRepository regionRepo,
        IMapper mapper)
        : ICountryService
    {
        public IEnumerable<CountryDto> GetAllCountries()
        {
            var countries = countryRepo.GetAll();

            return mapper
                .Map<IEnumerable<CountryDto>>(countries);
        }

        public CountryDto? GetCountryById(string id)
        {
            var country = countryRepo.GetById(id);

            if (country is null) return null;

            return mapper.Map<CountryDto>(country);
        }

        public IEnumerable<CountryDto> GetCountriesByRegion(
            decimal regionId)
        {
            var countries = countryRepo
                .GetByRegionId(regionId);

            return mapper
                .Map<IEnumerable<CountryDto>>(countries);
        }

        public void CreateCountry(CreateCountryDto dto)
        {
            if (!regionRepo.ExistsById(dto.RegionId))
                throw new ValidationException(
                    $"Region with id {dto.RegionId}" +
                    $" does not exist");

            dto.CountryId = dto.CountryId?
                .Trim().ToUpper() ?? string.Empty;

            if (countryRepo.ExistsById(dto.CountryId))
                throw new DuplicateException(
                    $"Country '{dto.CountryId}'" +
                    $" already exists");

            var country = mapper.Map<Country>(dto);

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

            if (!regionRepo.ExistsById(dto.RegionId))
                throw new ValidationException(
                    $"Region with id {dto.RegionId}" +
                    $" does not exist");

            mapper.Map(dto, country);

            countryRepo.Update(country);
            countryRepo.SaveChanges();
        }

        public IEnumerable<RegionDto> GetRegionsForDropdown()
        {
            var regions = regionRepo.GetAll();

            return mapper
                .Map<IEnumerable<RegionDto>>(regions);
        }
    }
}
