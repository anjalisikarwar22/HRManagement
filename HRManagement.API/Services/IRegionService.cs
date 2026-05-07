using HRManagement.API.DTOs;
namespace HRManagement.API.Services.Interfaces
{
    public interface IRegionService
    {
        IEnumerable<RegionDto> GetAllRegions();
        RegionDto GetRegionById(decimal id);

        void CreateRegion(CreateRegionDto dto);

        void UpdateRegion(decimal id, CreateRegionDto dto);
    }
}
