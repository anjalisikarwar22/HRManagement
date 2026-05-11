using HRManagement.API.DTOs.Location;
using HRManagement.API.Exceptions;
using HRManagement.API.Models;
using HRManagement.API.Repository;

namespace HRManagement.API.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repository;

        public LocationService(ILocationRepository repository)
        {
            _repository = repository;
        }


        public async Task<List<LocationDto>> GetAllAsync()
        {
            var locations = await _repository.GetAllAsync();

            return locations.Select(MapToDto).ToList();
        }

        public async Task<LocationDto> GetByIdAsync(decimal id)
        {
            var location = await _repository.GetByIdAsync(id);

            if (location == null)
                throw new NotFoundException("Location not found");

            return MapToDto(location);
        }



        public async Task CreateAsync(CreateLocationDto dto)
        {
            var maxLocationId = await _repository.GetMaxLocationIdAsync();

            var nextLocationId = (maxLocationId ?? 0) + 100;


            var location = new Location
            {
                LocationId = nextLocationId,
                City = dto.City,
                StreetAddress = dto.StreetAddress,
                PostalCode = dto.PostalCode,
                StateProvince = dto.StateProvince,
                CountryId = dto.CountryId
            };

            await _repository.AddAsync(location);

            await _repository.SaveChangesAsync();
        }


        public async Task UpdateAsync(decimal id, UpdateLocationDto dto)
        {
            var location = await _repository.GetByIdAsync(id);

            if (location == null)
            {
                throw new NotFoundException($"Location Id {id} not found" );
            }

            location.City = dto.City;
            location.StreetAddress = dto.StreetAddress;
            location.PostalCode = dto.PostalCode;
            location.StateProvince = dto.StateProvince;
            location.CountryId = dto.CountryId;

            await _repository.UpdateAsync(location);

            await _repository.SaveChangesAsync();
        }

        public async Task<List<LocationDto>> SearchAsync(string? city, string? state)
        {
            var locations = await _repository.SearchAsync(city, state);

            return locations.Select(MapToDto).ToList();
        }



        public async Task<List<DropdownDto>> GetDropdownAsync()
        {
            var locations = await _repository.GetAllAsync();

            return locations.Select(x => new DropdownDto
            {
                Id = x.LocationId,
                Name = x.City ?? "N/A"
            }).ToList();
        }

        public async Task<List<LocationDto>> GetByCountryAsync(string countryId)
        {
            var locations = await _repository.GetByCountryAsync(countryId);

            return locations.Select(MapToDto).ToList();
        }

        public async Task<LocationDto> GetByCityAsync(string city)
        {
            var location = await _repository.GetByCityAsync(city);

            if (location == null)
                throw new NotFoundException("Location not found");

            return MapToDto(location);
        }

        public async Task<int> CountAsync()
        {
            return await _repository.CountAsync();
        }

        public async Task<List<string>> GetDistinctStatesAsync()
        {
            return await _repository.GetDistinctStatesAsync();
        }



        private static LocationDto MapToDto(Location x)
        {
            return new LocationDto
            {
                LocationId = x.LocationId,

                City = string.IsNullOrWhiteSpace(x.City)
                    ? "N/A"
                    : x.City,

                StreetAddress = string.IsNullOrWhiteSpace(x.StreetAddress)
                    ? "N/A"
                    : x.StreetAddress,

                PostalCode = string.IsNullOrWhiteSpace(x.PostalCode)
                    ? "N/A"
                    : x.PostalCode,

                StateProvince = string.IsNullOrWhiteSpace(x.StateProvince)
                    ? "N/A"
                    : x.StateProvince,

                CountryId = string.IsNullOrWhiteSpace(x.CountryId)
                    ? "N/A"
                    : x.CountryId,

                CountryName = string.IsNullOrWhiteSpace(x.Country?.CountryName)
                    ? "N/A"
                    : x.Country.CountryName
            };
        }
    }
}
