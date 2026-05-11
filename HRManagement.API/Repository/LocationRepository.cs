using HRManagement.API.Data;
using HRManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.API.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly HRContext _context;

        public LocationRepository(HRContext context)
        {
            _context = context;
        }

        public async Task<List<Location>> GetAllAsync()
        {
            return await _context.Locations
                .Include(x => x.Country)
                .ToListAsync();
        }

public async Task<Location?> GetByIdAsync(decimal id)
        {
            return await _context.Locations
                .Include(x => x.Country)
                .FirstOrDefaultAsync(x => x.LocationId == id);
        }

public async Task AddAsync(Location location)
        {
            await _context.Locations.AddAsync(location);
        }

public async Task UpdateAsync(Location location)
        {
            _context.Locations.Update(location);
            await Task.CompletedTask;
        }

public async Task<List<Location>> SearchAsync(string? city, string? state)
        {
            var query = _context.Locations
                .Include(x => x.Country)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(x => x.City != null &&
                    x.City.ToLower().Contains(city.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(state))
            {
                query = query.Where(x => x.StateProvince != null &&
                    x.StateProvince.ToLower().Contains(state.ToLower()));
            }

            return await query.ToListAsync();
        }

public async Task<int> CountAsync()
        {
            return await _context.Locations.CountAsync();
        }

public async Task<List<Location>> GetByCountryAsync(string countryId)
        {
            return await _context.Locations
                .Where(x => x.CountryId == countryId)
                .Include(x => x.Country)
                .ToListAsync();
        }

public async Task<Location?> GetByCityAsync(string city)
        {
            return await _context.Locations
                .Include(x => x.Country)
                .FirstOrDefaultAsync(x =>
                    x.City != null &&
                    x.City.ToLower() == city.ToLower());
        }

public async Task<List<string>> GetDistinctStatesAsync()
        {
            return await _context.Locations
                .Where(x => x.StateProvince != null)
                .Select(x => x.StateProvince!)
                .Distinct()
                .ToListAsync();
        }

public async Task<decimal?> GetMaxLocationIdAsync()
        {
            return await _context.Locations
                .MaxAsync(x => (decimal?)x.LocationId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
