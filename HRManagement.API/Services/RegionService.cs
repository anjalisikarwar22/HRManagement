using HRManagement.API.DTOs;
using HRManagement.API.Exceptions;
using HRManagement.API.Models;
using HRManagement.API.Repository;
using HRManagement.API.Services;

namespace HRManagement.API.Services
{
    public class RegionService(IRegionRepository regionRepo)
        : IRegionService
    {
        public IEnumerable<RegionDto> GetAllRegions()
            => regionRepo.GetAll()
                .Select(r => new RegionDto
                {
                    RegionId = r.RegionId,
                    RegionName = r.RegionName
                });

        public RegionDto? GetRegionById(decimal id)
        {
            var region = regionRepo.GetById(id);
            if (region is null) return null;

            return new RegionDto
            {
                RegionId = region.RegionId,
                RegionName = region.RegionName
            };
        }

        public void CreateRegion(CreateRegionDto dto)
        {
            var duplicate = regionRepo.GetAll()
                .FirstOrDefault(r =>
                    string.Equals(
                        r.RegionName,
                        dto.RegionName,
                        StringComparison.OrdinalIgnoreCase));

            if (duplicate is not null)
                throw new DuplicateException(
                    $"Region '{dto.RegionName}' already exists");

            regionRepo.Add(new Region
            {
                RegionName = dto.RegionName
            });
            regionRepo.SaveChanges();
        }

        public void UpdateRegion(decimal id,
            CreateRegionDto dto)
        {
            var region = regionRepo.GetById(id);

            if (region is null)
                throw new NotFoundException(
                    $"Region with id {id} not found");

            region.RegionName = dto.RegionName;
            regionRepo.Update(region);
            regionRepo.SaveChanges();
        }
    }
}