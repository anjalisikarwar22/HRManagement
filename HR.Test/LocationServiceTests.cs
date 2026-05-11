using HRManagement.API.DTOs.Location;
using HRManagement.API.Exceptions;
using HRManagement.API.Models;
using HRManagement.API.Repository;
using HRManagement.API.Services;
using Moq;

namespace HR.Test
{
    public class LocationServiceTests
    {
        private readonly Mock<ILocationRepository> _repositoryMock;
        private readonly LocationService _service;

        public LocationServiceTests()
        {
            _repositoryMock = new Mock<ILocationRepository>();
            _service = new LocationService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnLocation()
        {
            var location = new Location
            {
                LocationId = 1000,
                City = "Seattle"
            };

            _repositoryMock.Setup(x => x.GetByIdAsync(1000)).ReturnsAsync(location);

            var result = await _service.GetByIdAsync(1000);

            Assert.NotNull(result);
            Assert.Equal("Seattle", result.City);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFound()
        {
            _repositoryMock.Setup(x => x.GetByIdAsync(9999))
                .ReturnsAsync((Location?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(9999));
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateLocation()
        {
            var dto = new CreateLocationDto
            {
                City = "Delhi",
                StreetAddress = "Street 1",
                PostalCode = "110001",
                StateProvince = "Delhi",
                CountryId = "IN"
            };

            _repositoryMock.Setup(x => x.GetMaxLocationIdAsync())
                .ReturnsAsync(2200);

            await _service.CreateAsync(dto);

            _repositoryMock.Verify(
                x => x.AddAsync(It.Is<Location>(location =>
                    location.LocationId == 2300 &&
                    location.City == "Delhi" &&
                    location.CountryId == "IN")),
                Times.Once);
            _repositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenNoLocationsExist_ShouldStartAt100()
        {
            var dto = new CreateLocationDto
            {
                City = "Delhi",
                CountryId = "IN"
            };

            _repositoryMock.Setup(x => x.GetMaxLocationIdAsync())
                .ReturnsAsync((decimal?)null);

            await _service.CreateAsync(dto);

            _repositoryMock.Verify(
                x => x.AddAsync(It.Is<Location>(location => location.LocationId == 100)),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateLocation()
        {
            var existingLocation = new Location
            {
                LocationId = 1000,
                City = "Old City"
            };

            var dto = new UpdateLocationDto
            {
                City = "New Delhi",
                StreetAddress = "Street 1",
                PostalCode = "110001",
                StateProvince = "Delhi",
                CountryId = "IN"
            };

            _repositoryMock.Setup(x => x.GetByIdAsync(1000))
                .ReturnsAsync(existingLocation);

            await _service.UpdateAsync(1000, dto);

            Assert.Equal("New Delhi", existingLocation.City);
            _repositoryMock.Verify(x => x.UpdateAsync(existingLocation), Times.Once);
            _repositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUpdateFails()
        {
            var location = new Location
            {
                LocationId = 1000,
                City = "Old City"
            };

            var dto = new UpdateLocationDto
            {
                City = "Delhi"
            };

            _repositoryMock.Setup(x => x.GetByIdAsync(1000))
                .ReturnsAsync(location);
            _repositoryMock.Setup(x => x.UpdateAsync(location))
                .ThrowsAsync(new Exception("Database error"));

            await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1000, dto));
        }

        [Fact]
        public async Task GetByCityAsync_ShouldReturnLocation()
        {
            var location = new Location
            {
                LocationId = 1000,
                City = "Seattle"
            };

            _repositoryMock.Setup(x => x.GetByCityAsync("Seattle"))
                .ReturnsAsync(location);

            var result = await _service.GetByCityAsync("Seattle");

            Assert.NotNull(result);
            Assert.Equal("Seattle", result.City);
        }

        [Fact]
        public async Task GetByCityAsync_ShouldThrowNotFound()
        {
            _repositoryMock.Setup(x => x.GetByCityAsync("Unknown"))
                .ReturnsAsync((Location?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByCityAsync("Unknown"));
        }
    }
}
