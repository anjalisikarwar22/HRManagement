using HRManagement.API.DTOs;
using HRManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
       
        private readonly IRegionService _regionService;

        public RegionController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        [HttpGet]
        public IActionResult GetAllRegions()
        {
            var regions = _regionService.GetAllRegions();
            return Ok(regions);
        }

     
        [HttpGet("{id}")]
        public IActionResult GetRegionById(decimal id)
        {
            var region = _regionService.GetRegionById(id);

            if (region == null)
                return NotFound(
                    $"Region with id {id} not found");

            return Ok(region);
        }

       
        [HttpGet("{id}/countries")]
        public IActionResult GetCountriesByRegion(
            decimal id,
            [FromServices] ICountryService countryService)
        {
            var region = _regionService.GetRegionById(id);
            if (region == null)
                return NotFound(
                    $"Region with id {id} not found");

            var countries = countryService
                                .GetCountriesByRegion(id);

            return Ok(countries);
        }

        [HttpPost]
        public IActionResult CreateRegion(
            [FromBody] CreateRegionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _regionService.CreateRegion(dto);
                return Ok("Region created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRegion(
            decimal id,
            [FromBody] CreateRegionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _regionService.UpdateRegion(id, dto);
                return Ok("Region updated successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
