using AutoMapper;
using HRManagement.API.DTOs;
using HRManagement.API.Exceptions;
using HRManagement.API.Models;
using HRManagement.API.Repository;

namespace HRManagement.API.Services
{
    public class RegionService(
        IRegionRepository regionRepo,
        IMapper mapper)
        : IRegionService
    {
        public IEnumerable<RegionDto> GetAllRegions()
        {
            var regions = regionRepo.GetAll();

            return mapper
                .Map<IEnumerable<RegionDto>>(regions);
        }

        public RegionDto? GetRegionById(decimal id)
        {
            var region = regionRepo.GetById(id);

            if (region is null) return null;

            return mapper.Map<RegionDto>(region);
        }

        public void CreateRegion(CreateRegionDto dto)
        {
            if (regionRepo.ExistsByName(
                    dto.RegionName ?? string.Empty))
                throw new DuplicateException(
                    $"Region '{dto.RegionName}' already exists");

            var region = mapper.Map<Region>(dto);

            regionRepo.Add(region);
            regionRepo.SaveChanges();
        }

        public void UpdateRegion(
            decimal id, CreateRegionDto dto)
        {
            var region = regionRepo.GetById(id);

            if (region is null)
                throw new NotFoundException(
                    $"Region with id {id} not found");

            mapper.Map(dto, region);

            regionRepo.Update(region);
            regionRepo.SaveChanges();
        }
    }
}
