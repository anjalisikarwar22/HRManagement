using HRManagement.API.DTOs;
using HRManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }
        [HttpGet]
        public IActionResult GetAllCountries()
        {
            var countries = _countryService.GetAllCountries();
            return Ok(countries);
        }

        
        [HttpGet("{id}")]
        public IActionResult GetCountryById(string id)
        {
            var country = _countryService.GetCountryById(id);

            if (country == null)
                return NotFound(
                    $"Country with code '{id}' not found");

            return Ok(country);
        }

      
        [HttpGet("byregion/{regionId}")]
        public IActionResult GetCountriesByRegion(
            decimal regionId)
        {
            var countries = _countryService
                                .GetCountriesByRegion(regionId);
            return Ok(countries);
        }

        [HttpGet("regions")]
        public IActionResult GetRegionsDropdown()
        {
            var regions = _countryService
                              .GetRegionsForDropdown();
            return Ok(regions);
        }

        [HttpPost]
        public IActionResult CreateCountry(
            [FromBody] CreateCountryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _countryService.CreateCountry(dto);
                return Ok("Country created successfully");
            }
            catch (Exception ex)
            {
         
                return BadRequest(ex.Message);
            }
        }

        
        [HttpPut("{id}")]
        public IActionResult UpdateCountry(
            string id,
            [FromBody] CreateCountryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _countryService.UpdateCountry(id, dto);
                return Ok("Country updated successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
