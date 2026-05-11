using HRManagement.API.Data;
using HRManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.API.Repository
{
    public class CountryRepository(HRContext db)
        : ICountryRepository
    {
        public IEnumerable<Country> GetAll()
            => db.Countries
                 .Include(c => c.Region)
                 .ToList();
        public Country? GetById(string id)
            => db.Countries
                 .Include(c => c.Region)
                 .FirstOrDefault(c =>
                     c.CountryId.Trim() == id.Trim());
        public IEnumerable<Country> GetByRegionId(
            decimal regionId)
            => db.Countries
                 .Include(c => c.Region)
                 .Where(c => c.RegionId == regionId)
                 .ToList();

        public bool ExistsById(string id)
            => db.Countries.Any(c =>
                c.CountryId.Trim() == id.Trim());

        public bool RegionExists(decimal regionId)
            => db.Regions.Any(r =>
                r.RegionId == regionId);
        public void Add(Country country)
            => db.Countries.Add(country);
        public void Update(Country country)
            => db.Entry(country).State =
                EntityState.Modified;
        public void SaveChanges()
            => db.SaveChanges();
    }
}