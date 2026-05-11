using HRManagement.API.Data;
using HRManagement.API.Models;
using HRManagement.API.Repository;
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

        public void Add(Country country)
            => db.Countries.Add(country);

        public void Update(Country country)
            => db.Entry(country).State =
                EntityState.Modified;

        public void SaveChanges()
            => db.SaveChanges();
    }
}