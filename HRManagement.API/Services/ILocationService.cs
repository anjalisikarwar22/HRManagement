using HRManagement.API.DTOs.Location;

namespace HRManagement.API.Services
{
    public interface ILocationService
    {
        Task<List<LocationDto>> GetAllAsync();

        Task<LocationDto> GetByIdAsync(decimal id);

        Task CreateAsync(LocationRequestDto dto);

        Task UpdateAsync(decimal id, LocationRequestDto dto);

        Task<List<LocationDto>> SearchAsync(string? city, string? state);

        Task<List<DropdownDto>> GetDropdownAsync();

        Task<List<LocationDto>> GetByCountryAsync(string countryId);

        Task<LocationDto> GetByCityAsync(string city);

        Task<int> CountAsync();

        Task<List<string>> GetDistinctStatesAsync();

    }
}
