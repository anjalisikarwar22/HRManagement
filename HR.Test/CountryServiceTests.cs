using AutoMapper;
using HRManagement.API.DTOs;
using HRManagement.API.Exceptions;
using HRManagement.API.Models;
using HRManagement.API.Repository;
using HRManagement.API.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Diagnostics.Metrics;

namespace HR.Test
{
    [TestClass]
    public class CountryServiceTests    
    {
        private Mock<ICountryRepository>? _mockCountryRepo;
        private Mock<IRegionRepository>? _mockRegionRepo;
        private Mock<IMapper>? _mockMapper;
        private CountryService? _service;

        [TestInitialize]
        public void Setup()
        {
            _mockCountryRepo = new();
            _mockRegionRepo = new();
            _mockMapper = new();

            _service = new(
                _mockCountryRepo.Object,
                _mockRegionRepo.Object,
                _mockMapper.Object);
        }


        [TestMethod]
        public void GetAllCountries_WhenCalled_ReturnsCorrectDtos()
        {
            var fakeCountries = new List<Country>
            {
                new()
                {
                    CountryId   = "US  ",
                    CountryName = "United States",
                    RegionId    = 2,
                    Region      = new()
                    {
                        RegionId   = 2,
                        RegionName = "Americas"
                    }
                },
                new()
                {
                    CountryId   = "IN  ",
                    CountryName = "India",
                    RegionId    = 3,
                    Region      = new()
                    {
                        RegionId   = 3,
                        RegionName = "Asia"
                    }
                }
            };

            var fakeDtos = new List<CountryDto>
            {
                new()
                {
                    CountryId   = "US",
                    CountryName = "United States",
                    RegionId    = 2,
                    RegionName  = "Americas"
                },
                new()
                {
                    CountryId   = "IN",
                    CountryName = "India",
                    RegionId    = 3,
                    RegionName  = "Asia"
                }
            };

            _mockCountryRepo!
                .Setup(r => r.GetAll())
                .Returns(fakeCountries);

            _mockMapper!
                .Setup(m => m.Map<IEnumerable<CountryDto>>(
                    fakeCountries))
                .Returns(fakeDtos);

            var result = _service!
                .GetAllCountries()
                .ToList();

            Assert.AreEqual(2, result.Count,
                "Should return 2 countries");

            Assert.AreEqual("US",
                result[0].CountryId,
                "CountryId should be US");

            Assert.AreEqual("Americas",
                result[0].RegionName,
                "RegionName should be Americas");

            Assert.AreEqual("Asia",
                result[1].RegionName,
                "Second country RegionName should be Asia");
        }

        [TestMethod]
        public void CreateCountry_WithValidData_UppercasesAndSaves()
        {
         
            var dto = new CreateCountryDto
            {
                CountryId = "pk",  
                CountryName = "Pakistan",
                RegionId = 3
            };

            _mockRegionRepo!
                .Setup(r => r.ExistsById(3))
                .Returns(true);

          
            _mockCountryRepo!
                .Setup(r => r.ExistsById("PK"))
                .Returns(false);

            _mockMapper!
                .Setup(m => m.Map<Country>(
                    It.IsAny<CreateCountryDto>()))
                .Returns(new Country
                {
                    CountryId = "PK",
                    CountryName = "Pakistan",
                    RegionId = 3
                });

            _service!.CreateCountry(dto);

            Assert.AreEqual("PK",
                dto.CountryId,
                "CountryId should be uppercased to PK");

            _mockCountryRepo.Verify(
                r => r.Add(It.IsAny<Country>()),
                Times.Once,
                "Add should be called once");

            _mockCountryRepo.Verify(
                r => r.SaveChanges(),
                Times.Once,
                "SaveChanges should be called once");
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void CreateCountry_WhenRegionNotFound_ThrowsValidationException()
        {
           
            var dto = new CreateCountryDto
            {
                CountryId = "XX",
                CountryName = "Test Country",
                RegionId = 999  
            };

            _mockRegionRepo!
                .Setup(r => r.ExistsById(999))
                .Returns(false);

            _service!.CreateCountry(dto);

        }


        [TestMethod]
        [ExpectedException(typeof(DuplicateException))]
        public void CreateCountry_WhenDuplicate_ThrowsDuplicateException()
        {
            var dto = new CreateCountryDto
            {
                CountryId = "US",  
                CountryName = "United States",
                RegionId = 2
            };

            _mockRegionRepo!
                .Setup(r => r.ExistsById(2))
                .Returns(true);

            _mockCountryRepo!
                .Setup(r => r.ExistsById("US"))
                .Returns(true);

            _service!.CreateCountry(dto);

        }

    }   
}