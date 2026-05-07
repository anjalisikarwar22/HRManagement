using HRManagement.API.Data;
using HRManagement.API.Models;
using HRManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.API.Repository
{
    public class RegionRepository : IRegionRepository

    {
        private readonly HRContext _db;

       
        public RegionRepository(HRContext db)
        {
            _db = db;
        }
        public IEnumerable<Region> GetAll()
        {
            return _db.Regions.ToList();
           
        }

        public Region GetById(decimal id)
        {
            return _db.Regions.Find(id);
          
        }

        public void Add(Region region)
        {
            _db.Regions.Add(region);
        }

        public void Update(Region region)
        {
            _db.Entry(region).State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
