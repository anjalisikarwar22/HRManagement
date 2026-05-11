using HRManagement.API.DTOs.Location;
using HRManagement.API.Exceptions;
using HRManagement.API.Models;
using HRManagement.API.Repository;
using HRManagement.API.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Test
{


    public class LocationServiceTests
    {
        private readonly
            Mock<ILocationRepository>
            _repositoryMock;

        private readonly
            LocationService
            _service;

        public LocationServiceTests()
        {
            _repositoryMock = new Mock<ILocationRepository>();

            _service =new LocationService(_repositoryMock.Object);
        }



        [Fact]
        public async Task
    GetByIdAsync_ShouldReturnLocation()
        {
            // Arrange

            var location = new Location
            {
                LocationId = 1000,
                City = "Seattle"
            };

            _repositoryMock.Setup(x =>x.GetByIdAsync(1000)).ReturnsAsync(location);

            // Act

            var result = await _service.GetByIdAsync(1000);

            // Assert

            Assert.NotNull(result);

            Assert.Equal( "Seattle", result.City);
        }


        [Fact]
        public async Task
        GetByIdAsync_ShouldThrowNotFound()
        {
            // Arrange

            _repositoryMock.Setup(x => x.GetByIdAsync(9999))
                           .ReturnsAsync((Location)null);

            // Act + Assert

            await Assert.ThrowsAsync<NotFoundException>(() =>_service.GetByIdAsync(9999));
        }



        [Fact]
        public async Task
        CreateAsync_ShouldCreateLocation()
        {
            // Arrange

            var dto =new CreateLocationDto{
                    LocationId = 2300,
                    City = "Delhi"
                };

            _repositoryMock.Setup(x =>x.GetByIdAsync(2300))
                           .ReturnsAsync((Location)null);

            _repositoryMock.Setup(x =>x.GetMaxLocationIdAsync())
                           .ReturnsAsync(2200);

            // Act

            await _service.CreateAsync(dto);

            // Assert

            _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Location>()),Times.Once);

            _repositoryMock.Verify(x => x.SaveChangesAsync(),Times.Once);
        }



        [Fact]
        public async Task
        CreateAsync_ShouldThrowDuplicateException()
        {
            // Arrange

            var dto = new CreateLocationDto{
                    LocationId = 1000
                };

            _repositoryMock.Setup(x =>x.GetByIdAsync(1000))
                .ReturnsAsync(new Location());

            // Act + Assert

            await Assert.ThrowsAsync<BadRequestException>(() =>_service.CreateAsync(dto));
        }




        [Fact]
        public async Task
        UpdateAsync_ShouldUpdateLocation()
        {
            // Arrange

            var existingLocation =new Location
                {
                    LocationId = 1000,
                    City = "Old City"
                };

            var dto =new UpdateLocationDto
                {
                    City = "New Delhi",
                    StreetAddress = "Street 1",
                    PostalCode = "110001",
                    StateProvince = "Delhi",
                    CountryId = "IN"
                };

            _repositoryMock.Setup(x => x.GetByIdAsync(1000))
                            .ReturnsAsync(existingLocation);

            // Act

            await _service.UpdateAsync(1000, dto);

            // Assert

            Assert.Equal( "New Delhi",existingLocation.City);

            _repositoryMock.Verify(x => x.UpdateAsync(existingLocation),Times.Once);

            _repositoryMock.Verify(x => x.SaveChangesAsync(),Times.Once);
        }


        [Fact]
        public async Task
        UpdateAsync_ShouldThrowException_WhenUpdateFails()
        {
            // Arrange

            var location =new Location
                {
                    LocationId = 1000,
                    City = "Old City"
                };

            var dto =new UpdateLocationDto
                {
                    City = "Delhi"
                };

            _repositoryMock.Setup(x => x.GetByIdAsync(1000))
                           .ReturnsAsync(location);

            _repositoryMock.Setup(x =>x.UpdateAsync(location))
                           .ThrowsAsync(new Exception("Database error"));

            // Act + Assert

            await Assert.ThrowsAsync<Exception>(() =>_service.UpdateAsync(1000,dto));
        }


        [Fact]
        public async Task
        GetByCityAsync_ShouldReturnLocation()
        {
            // Arrange

            var location =new Location
                {
                    LocationId = 1000,
                    City = "Seattle"
                };

            _repositoryMock.Setup(x =>x.GetByCityAsync("Seattle"))
                           .ReturnsAsync(location);

            // Act

            var result =await _service.GetByCityAsync("Seattle");

            // Assert

            Assert.NotNull(result);

            Assert.Equal("Seattle",result.City);
        }



        [Fact]
        public async Task
        GetByCityAsync_ShouldThrowNotFound()
        {
            // Arrange

            _repositoryMock.Setup(x =>x.GetByCityAsync("Unknown"))
                           .ReturnsAsync((Location)null);

            // Act + Assert

            await Assert.ThrowsAsync<NotFoundException>(() =>_service.GetByCityAsync("Unknown"));
        }
    }
}
