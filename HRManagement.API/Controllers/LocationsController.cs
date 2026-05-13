<<<<<<< Updated upstream
﻿using HRManagement.API.Services;
using Microsoft.AspNetCore.Mvc;
using HRManagement.API.Common;
using Microsoft.AspNetCore.Authorization;
=======
﻿using HRManagement.API.Common;
>>>>>>> Stashed changes
using HRManagement.API.DTOs.Location;

namespace HRManagement.API.Controllers
{
    [ApiController]
    [Route("api/locations")]
    public class LocationsController: ControllerBase
    {
        private readonly ILocationService _service;

        public LocationsController(ILocationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
<<<<<<< Updated upstream

            return Ok(new ApiResponse<object>(
                true,
                "Locations fetched successfully",
                data
            ));
=======
            return Ok(new ApiResponse<object>(true, "Locations fetched successfully", data));
>>>>>>> Stashed changes
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(decimal id)
        {
            var data = await _service.GetByIdAsync(id);
<<<<<<< Updated upstream

            return Ok(new ApiResponse<object>(
                true,
                "Location fetched successfully",
                data
            ));
        }

        [HttpPost]
        public async Task<IActionResult> Create(LocationRequestDto dto)
        {
            await _service.CreateAsync(dto);

            return Ok(new ApiResponse<object>(
                true,
                "Location created successfully",
                null
            ));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(decimal id, LocationRequestDto dto)
        {
            await _service.UpdateAsync(id, dto);

            return Ok(new ApiResponse<object>(
                true,
                "Location updated successfully",
                null
            ));
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? city,
            [FromQuery] string? state)
        {
            var data = await _service.SearchAsync(city, state);

            return Ok(new ApiResponse<object>(
                true,
                "Search completed",
                data
            ));
        }


=======
            return Ok(new ApiResponse<object>(true, "Location fetched successfully", data));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateLocationDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok(new ApiResponse<object>(true, "Location created successfully", null));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(decimal id, UpdateLocationDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new ApiResponse<object>(true, "Location updated successfully", null));
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? city, [FromQuery] string? state)
        {
            var data = await _service.SearchAsync(city, state);
            return Ok(new ApiResponse<object>(true, "Search completed", data));
        }

        [Authorize(Roles = "Admin")]
>>>>>>> Stashed changes
        [HttpGet("dropdown")]
        public async Task<IActionResult> Dropdown()
        {
            var data = await _service.GetDropdownAsync();
<<<<<<< Updated upstream

            return Ok(new ApiResponse<object>(
                true,
                "Dropdown fetched successfully",
                data
            ));
        }


=======
            return Ok(new ApiResponse<object>(true, "Dropdown fetched successfully", data));
        }

>>>>>>> Stashed changes
        [HttpGet("country/{countryId}")]
        public async Task<IActionResult> ByCountry(string countryId)
        {
            var data = await _service.GetByCountryAsync(countryId);
<<<<<<< Updated upstream

            return Ok(new ApiResponse<object>(
                true,
                "Locations fetched successfully",
                data
            ));
=======
            return Ok(new ApiResponse<object>(true, "Locations fetched successfully", data));
>>>>>>> Stashed changes
        }

        [HttpGet("city/{city}")]
        public async Task<IActionResult> ByCity(string city)
        {
            var data = await _service.GetByCityAsync(city);
<<<<<<< Updated upstream

            return Ok(new ApiResponse<object>(
                true,
                "Locations fetched successfully",
                data
            ));
        }


=======
            return Ok(new ApiResponse<object>(true, "Locations fetched successfully", data));
        }

        [Authorize(Roles = "Admin")]
>>>>>>> Stashed changes
        [HttpGet("count")]
        public async Task<IActionResult> Count()
        {
            var count = await _service.CountAsync();
<<<<<<< Updated upstream

            return Ok(new ApiResponse<object>(
                true,
                "Count fetched successfully",
                count
            ));
=======
            return Ok(new ApiResponse<object>(true, "Count fetched successfully", count));
>>>>>>> Stashed changes
        }


        [HttpGet("states")]
        public async Task<IActionResult> States()
        {
            var data = await _service.GetDistinctStatesAsync();
<<<<<<< Updated upstream

            return Ok(new ApiResponse<object>(
                true,
                "States fetched successfully",
                data
            ));
        }

=======
            return Ok(new ApiResponse<object>(true, "States fetched successfully", data));
        }
>>>>>>> Stashed changes
    }
}
