namespace HRManagement.API.DTOs.Location
{
    public class LocationDto
    {
        public decimal LocationId { get; set; }
        public string? StreetAddress { get; set; }
        public string? PostalCode { get; set; }
        public string City { get; set; } = string.Empty;
        public string? StateProvince { get; set; }
        public string? CountryId { get; set; }
        public string? CountryName { get; set; }
    }
}
