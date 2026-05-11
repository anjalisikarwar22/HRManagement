using HRManagement.API.Common;
using HRManagement.API.DTOs;
using HRManagement.API.Filters;
using HRManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HRManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LogActionFilter))]
    public class CountryController(
        ICountryService countryService,
        ILogger<CountryController> logger)
        : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllCountries()
        {
            try
            {
                var countries = countryService
                    .GetAllCountries();

                return Ok(new ApiResponse<IEnumerable < CountryDto >>
                {
                    Success = true,
                    Message = "Countries fetched successfully",
                    Data = countries
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error in GetAllCountries");

                return StatusCode(500,
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Error fetching countries",
                        Data = null
                    });
            }
        }

        [HttpGet("{id:alpha:minlength(2):maxlength(4)}")]
        public IActionResult GetCountryById(string id)
        {
            try
            {
                var country = countryService
                    .GetCountryById(id);

                if (country is null)
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Country '{id}' not found",
                        Data = null
                    });

                return Ok(new ApiResponse<CountryDto>
                {
                    Success = true,
                    Message = "Country fetched successfully",
                    Data = country
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error in GetCountryById {Id}", id);

                return StatusCode(500,
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Error fetching country",
                        Data = null
                    });
            }
        }

        [HttpGet("byregion/{regionId:decimal:min(1)}")]
        public IActionResult GetCountriesByRegion(
            decimal regionId)
        {
            try
            {
                var countries = countryService
                    .GetCountriesByRegion(regionId);

                return Ok(new ApiResponse<IEnumerable < CountryDto >>
                {
                    Success = true,
                    Message = "Countries fetched successfully",
                    Data = countries
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error in GetCountriesByRegion {Id}",
                    regionId);

                return StatusCode(500,
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Error fetching countries",
                        Data = null
                    });
            }
        }

        [HttpGet("regions")]
        public IActionResult GetRegionsDropdown()
        {
            try
            {
                var regions = countryService
                    .GetRegionsForDropdown();

                return Ok(new ApiResponse<IEnumerable < RegionDto >>
                {
                    Success = true,
                    Message = "Regions fetched successfully",
                    Data = regions
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error in GetRegionsDropdown");

                return StatusCode(500,
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Error fetching regions",
                        Data = null
                    });
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilter))]
        public IActionResult CreateCountry(
            [FromBody] CreateCountryDto dto)
        {
            try
            {
                countryService.CreateCountry(dto);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Country created successfully",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error in CreateCountry");

                return StatusCode(500,
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Error creating country",
                        Data = null
                    });
            }
        }

        [HttpPut("{id:alpha:minlength(2):maxlength(4)}")]
        [ServiceFilter(typeof(ValidationFilter))]
        public IActionResult UpdateCountry(
            string id,
            [FromBody] CreateCountryDto dto)
        {
            try
            {
                countryService.UpdateCountry(id, dto);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Country updated successfully",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error in UpdateCountry {Id}", id);

                return StatusCode(500,
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Error updating country",
                        Data = null
                    });
            }
        }
    }
}