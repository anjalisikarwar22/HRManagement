using HRManagement.API.Models;

namespace HRManagement.API.Repository
{
    public interface ILocationRepository
    {
        Task<List<Location>> GetAllAsync();

        Task<Location?> GetByIdAsync(decimal id);

        Task AddAsync(Location location);

        Task UpdateAsync(Location location);

        Task<List<Location>> SearchAsync(string? city, string? state);

        Task<int> CountAsync();

        Task<List<Location>> GetByCountryAsync(string countryId);

        Task<Location?> GetByCityAsync(string city);

        Task<List<string>> GetDistinctStatesAsync();

        Task<decimal?> GetMaxLocationIdAsync();

        Task SaveChangesAsync();
    }
}
