using HRManagement.API.Models;

namespace HRManagement.API.Repositories.Interfaces
{
    public interface IRegionRepository
    {
        IEnumerable<Region> GetAll();

        Region GetById(decimal id);

        void Add(Region region);

        void Update(Region region);
        void SaveChanges();
    }
}
