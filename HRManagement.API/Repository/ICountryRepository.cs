using HRManagement.API.Models;

namespace HRManagement.API.Repository
{
    public interface ICountryRepository
    {
        IEnumerable<Country> GetAll();
        Country? GetById(string id);
        IEnumerable<Country> GetByRegionId(decimal regionId);
        void Add(Country country);
        void Update(Country country);
        void SaveChanges();
    }
}