// HRManagement.API/Repository/RegionRepository.cs
using HRManagement.API.Data;
using HRManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.API.Repository
{
    public class RegionRepository(HRContext db)
        : IRegionRepository
    {
        public IEnumerable<Region> GetAll()
            => db.Regions.ToList();

        public Region? GetById(decimal id)
            => db.Regions.Find(id);
        public bool ExistsByName(string name)
            => db.Regions.Any(r =>
                r.RegionName != null &&
                r.RegionName.ToLower() == name.ToLower());
        public bool ExistsById(decimal id)
            => db.Regions.Any(r => r.RegionId == id);
        public void Add(Region region)
            => db.Regions.Add(region);
        public void Update(Region region)
            => db.Entry(region).State =
                EntityState.Modified;
        public void SaveChanges()
            => db.SaveChanges();
    }
}