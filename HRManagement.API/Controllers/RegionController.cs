using HRManagement.API.Common;
using HRManagement.API.DTOs;
using HRManagement.API.Filters;
using HRManagement.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HRManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LogActionFilter))]
    public class RegionController(
        IRegionService regionService,
        ILogger<RegionController> logger)
        : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllRegions()
        {
            try
            {
                var regions = regionService.GetAllRegions();

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
                    "Error in GetAllRegions");

                return StatusCode(500,
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Error fetching regions",
                        Data = null
                    });
            }
        }

        [HttpGet("{id:decimal}")]
        public IActionResult GetRegionById(decimal id)
        {
            try
            {
                var region = regionService
                    .GetRegionById(id);

                if (region is null)
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Region with id {id}" +
                                  $" not found",
                        Data = null
                    });

                return Ok(new ApiResponse<RegionDto>
                {
                    Success = true,
                    Message = "Region fetched successfully",
                    Data = region
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error in GetRegionById {Id}", id);

                return StatusCode(500,
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Error fetching region",
                        Data = null
                    });
            }
        }

        [HttpGet("{id:decimal}/countries")]
        public IActionResult GetCountriesByRegion(
            decimal id,
            [FromServices] ICountryService countryService)
        {
            try
            {
                var region = regionService
                    .GetRegionById(id);

                if (region is null)
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Region with id {id}" +
                                  $" not found",
                        Data = null
                    });

                var countries = countryService
                    .GetCountriesByRegion(id);

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
                    "Error in GetCountriesByRegion {Id}", id);

                return StatusCode(500,
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Error fetching countries",
                        Data = null
                    });
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilter))]
        public IActionResult CreateRegion(
            [FromBody] CreateRegionDto dto)
        {
            try
            {
                regionService.CreateRegion(dto);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Region created successfully",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error in CreateRegion");

                return StatusCode(500,
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Error creating region",
                        Data = null
                    });
            }
        }

        [HttpPut("{id:decimal}")]
        [ServiceFilter(typeof(ValidationFilter))]
        public IActionResult UpdateRegion(
            decimal id,
            [FromBody] CreateRegionDto dto)
        {
            try
            {
                regionService.UpdateRegion(id, dto);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Region updated successfully",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error in UpdateRegion {Id}", id);

                return StatusCode(500,
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Error updating region",
                        Data = null
                    });
            }
        }
    }
}