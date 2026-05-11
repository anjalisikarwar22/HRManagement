using System.ComponentModel.DataAnnotations;

namespace HRManagement.API.DTOs.Location
{
    public class LocationRequestDto
    {
        public string City { get; set; } = string.Empty;

        public string? StreetAddress { get; set; }

        public string? PostalCode { get; set; }

        public string? StateProvince { get; set; }

        public string? CountryId { get; set; }
    }
}
