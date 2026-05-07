using HRManagement.API.DTOs;
using HRManagement.API.Models;
using HRManagement.API.Repositories.Interfaces;
using HRManagement.API.Services.Interfaces;
namespace HRManagement.API.Services
{
    public class RegionService:IRegionService
    {
        private readonly IRegionRepository _regionRepo;
        public RegionService(IRegionRepository regionRepo)
        {
            _regionRepo = regionRepo;
        }

        public IEnumerable<RegionDto> GetAllRegions()
        {
            var regions = _regionRepo.GetAll();
            return regions.Select(r => new RegionDto
            {
                RegionId=r.RegionId,
                RegionName=r.RegionName

            });
        }
        public RegionDto GetRegionById(decimal id)
        {
            var region=_regionRepo.GetById(id);
            if (region == null)
            {
                return null;
            }
            return new RegionDto
            {
                RegionId = region.RegionId,
                RegionName = region.RegionName
            };
        }
        public void CreateRegion(CreateRegionDto dto)
        {
            var dupli = _regionRepo.GetAll().FirstOrDefault(r => r.RegionName.ToLower() == dto.RegionName.ToLower());
            if(dupli!= null)
            {
                throw new Exception("Region with the same name already exists");
            }
            var region = new Region
            {
                RegionName = dto.RegionName
            };
            _regionRepo.Add(region);
            _regionRepo.SaveChanges();
        }
        public void UpdateRegion(decimal id, CreateRegionDto dto)
        {
            var region = _regionRepo.GetById(id);
            if(region==null)
            {
              throw new Exception($"Region with id {id} not found");
            }
            region.RegionName = dto.RegionName;
            _regionRepo.Update(region);     
            _regionRepo.SaveChanges();
        }
    }
}
