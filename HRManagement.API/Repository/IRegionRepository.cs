using HRManagement.API.Models;

namespace HRManagement.API.Repository
{
    public interface IRegionRepository
    {
        IEnumerable<Region> GetAll();
        Region? GetById(decimal id);
        void Add(Region region);
        void Update(Region region);
        void SaveChanges();
        bool ExistsByName(string name);
        bool ExistsById(decimal id);
    }
}