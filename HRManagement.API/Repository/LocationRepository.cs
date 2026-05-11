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
<<<<<<< HEAD
=======

>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
        public async Task<List<Location>> GetAllAsync()
        {
            return await _context.Locations
                .Include(x => x.Country)
                .ToListAsync();
        }
<<<<<<< HEAD
        public async Task<Location?> GetByIdAsync(decimal id)
=======

public async Task<Location?> GetByIdAsync(decimal id)
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
        {
            return await _context.Locations
                .Include(x => x.Country)
                .FirstOrDefaultAsync(x => x.LocationId == id);
        }
<<<<<<< HEAD
        public async Task AddAsync(Location location)
        {
            await _context.Locations.AddAsync(location);
        }
        public async Task UpdateAsync(Location location)
=======

public async Task AddAsync(Location location)
        {
            await _context.Locations.AddAsync(location);
        }

public async Task UpdateAsync(Location location)
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
        {
            _context.Locations.Update(location);
            await Task.CompletedTask;
        }
<<<<<<< HEAD
        public async Task<List<Location>> SearchAsync(string? city, string? state)
=======

public async Task<List<Location>> SearchAsync(string? city, string? state)
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
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
<<<<<<< HEAD
        public async Task<int> CountAsync()
        {
            return await _context.Locations.CountAsync();
        }
        public async Task<List<Location>> GetByCountryAsync(string countryId)
=======

public async Task<int> CountAsync()
        {
            return await _context.Locations.CountAsync();
        }

public async Task<List<Location>> GetByCountryAsync(string countryId)
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
        {
            return await _context.Locations
                .Where(x => x.CountryId == countryId)
                .Include(x => x.Country)
                .ToListAsync();
        }
<<<<<<< HEAD
        public async Task<Location?> GetByCityAsync(string city)
=======

public async Task<Location?> GetByCityAsync(string city)
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
        {
            return await _context.Locations
                .Include(x => x.Country)
                .FirstOrDefaultAsync(x =>
                    x.City != null &&
                    x.City.ToLower() == city.ToLower());
        }
<<<<<<< HEAD
        public async Task<List<string>> GetDistinctStatesAsync()
=======

public async Task<List<string>> GetDistinctStatesAsync()
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
        {
            return await _context.Locations
                .Where(x => x.StateProvince != null)
                .Select(x => x.StateProvince!)
                .Distinct()
                .ToListAsync();
        }
<<<<<<< HEAD
        public async Task<decimal?> GetMaxLocationIdAsync()
=======

public async Task<decimal?> GetMaxLocationIdAsync()
>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
        {
            return await _context.Locations
                .MaxAsync(x => (decimal?)x.LocationId);
        }
<<<<<<< HEAD
=======

>>>>>>> 414c489704f573054bb98c6e424753a252d8dd96
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

