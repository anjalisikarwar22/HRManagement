using AutoMapper;
using HRManagement.API.DTOs.Location;
using HRManagement.API.Exceptions;
using HRManagement.API.Models;
using HRManagement.API.Repository;

namespace HRManagement.API.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repository;
        private readonly IMapper _mapper;

        public LocationService(ILocationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<List<LocationDto>> GetAllAsync()
        {
            var locations = await _repository.GetAllAsync();

            return _mapper.Map<List<LocationDto>>(locations);
        }

        public async Task<LocationDto> GetByIdAsync(decimal id)
        {
            var location = await _repository.GetByIdAsync(id);

            if (location == null)
                throw new NotFoundException("Location not found");

            return _mapper.Map<LocationDto>(location);
        }



        public async Task CreateAsync(LocationRequestDto dto)
        {
            var maxLocationId = await _repository.GetMaxLocationIdAsync();

            var location =_mapper.Map<Location>(dto);

            location.LocationId =(maxLocationId ?? 0) + 100;


            await _repository.AddAsync(location);

            await _repository.SaveChangesAsync();
        }


        public async Task UpdateAsync(decimal id, LocationRequestDto dto)
        {
            var location = await _repository.GetByIdAsync(id);

            if (location == null)
            {
                throw new NotFoundException(
                    $"Location Id {id} not found"
                );
            }

            _mapper.Map(dto, location);

            await _repository.UpdateAsync(location);

            await _repository.SaveChangesAsync();
        }

        public async Task<List<LocationDto>> SearchAsync(string? city, string? state)
        {
            var locations = await _repository.SearchAsync(city, state);

            return _mapper.Map<List<LocationDto>>(locations);
        }



        public async Task<List<DropdownDto>> GetDropdownAsync()
        {
            var locations = await _repository.GetAllAsync();

            return _mapper.Map<List<DropdownDto>>(locations);
        }

        public async Task<List<LocationDto>> GetByCountryAsync(string countryId)
        {
            var locations = await _repository.GetByCountryAsync(countryId);

            return _mapper.Map<List<LocationDto>>(locations);
        }

        public async Task<LocationDto> GetByCityAsync(string city)
        {
            var location = await _repository.GetByCityAsync(city);

            if (location == null)
                throw new NotFoundException("Location not found");

            return _mapper.Map<LocationDto>(location);
        }

        public async Task<int> CountAsync()
        {
            return await _repository.CountAsync();
        }

        public async Task<List<string>> GetDistinctStatesAsync()
        {
            return await _repository.GetDistinctStatesAsync();
        }
    }
}
