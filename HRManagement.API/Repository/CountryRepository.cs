using HRManagement.API.Data;
using HRManagement.API.Models;
using HRManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace HRManagement.API.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly HRContext _db;
        public CountryRepository(HRContext db)
        {
            _db = db;
        }
        public IEnumerable<Country> GetAll()
        {
            return _db.Countries.Include(c => c.Region).ToList();
        }
        public Country GetById(string id)
        {
            return _db.Countries.Include(c => c.Region).FirstOrDefault(c => c.CountryId.Trim() == id.Trim());
        }
        public IEnumerable<Country> GetByRegionId(decimal regionId)
        {
            return _db.Countries.Include(c => c.Region).Where(c => c.RegionId == regionId).ToList();
        }

        public void Add(Country country)
        {
            _db.Countries.Add(country);
        }
        public void Update(Country country)
        {
            _db.Entry(country).State = EntityState.Modified;
        }
        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }

    }
