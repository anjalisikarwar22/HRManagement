using HRManagement.API.DTOs;
using HRManagement.API.Exceptions;
using HRManagement.API.Models;
using HRManagement.API.Repository;
using HRManagement.API.Services;
using Moq;

namespace HR.Test
{
    public class CountryServiceTests
    {
        private Mock<ICountryRepository>? _mockCountryRepo;
        private Mock<IRegionRepository>? _mockRegionRepo;
        private CountryService? _service;

        public CountryServiceTests()
        {
            _mockCountryRepo = new Mock<ICountryRepository>();
            _mockRegionRepo = new Mock<IRegionRepository>();
            _service = new CountryService(
                _mockCountryRepo.Object,
                _mockRegionRepo.Object);
        }

        [Fact]
        public void GetAllCountries_WhenCalled_ReturnsCorrectDtos()
        {
            var fakeData = new List<Country>
            {
                new Country
                {
                    CountryId = "US  ",
                    CountryName = "United States",
                    RegionId = 2,
                    Region = new Region { RegionId = 2, RegionName = "Americas" }
                },
                new Country
                {
                    CountryId = "IN  ",
                    CountryName = "India",
                    RegionId = 3,
                    Region = new Region { RegionId = 3, RegionName = "Asia" }
                }
            };
            _mockCountryRepo!.Setup(r => r.GetAll()).Returns(fakeData);

            var result = _service!.GetAllCountries().ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("US", result[0].CountryId);
            Assert.Equal("Americas", result[0].RegionName);
        }

        [Fact]
        public void CreateCountry_WithValidData_UppercasesAndSaves()
        {
            var dto = new CreateCountryDto
            {
                CountryId = "pk",
                CountryName = "Pakistan",
                RegionId = 3
            };
            _mockRegionRepo!.Setup(r => r.GetById(3))
                .Returns(new Region { RegionId = 3, RegionName = "Asia" });
            _mockCountryRepo!.Setup(r => r.GetById(It.IsAny<string>()))
                .Returns((Country?)null);

            _service!.CreateCountry(dto);

            _mockCountryRepo.Verify(
                r => r.Add(It.Is<Country>(c => c.CountryId == "PK")),
                Times.Once,
                "Add should be called with uppercased CountryId PK");
            _mockCountryRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void CreateCountry_WhenDuplicate_ThrowsDuplicateException()
        {
            var dto = new CreateCountryDto
            {
                CountryId = "US",
                CountryName = "United States",
                RegionId = 2
            };
            _mockRegionRepo!.Setup(r => r.GetById(2))
                .Returns(new Region { RegionId = 2, RegionName = "Americas" });
            _mockCountryRepo!.Setup(r => r.GetById("US"))
                .Returns(new Country { CountryId = "US", CountryName = "United States" });

            Assert.Throws<DuplicateException>(() => _service!.CreateCountry(dto));
        }
    }
}
