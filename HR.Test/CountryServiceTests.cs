using Microsoft.VisualStudio.TestTools.UnitTesting;
﻿using HRManagement.API.DTOs;
using HRManagement.API.Exceptions;
using HRManagement.API.Models;
using HRManagement.API.Repository;
using HRManagement.API.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace HR.Test
{
    [TestClass]
    public class CountryServiceTests
    {
        private Mock<ICountryRepository>? _mockCountryRepo;
        private Mock<IRegionRepository>? _mockRegionRepo;
        private CountryService? _service;

        [TestInitialize]
        public void Setup()
        {
            _mockCountryRepo = new Mock<ICountryRepository>();
            _mockRegionRepo = new Mock<IRegionRepository>();
            _service = new CountryService(
                _mockCountryRepo.Object,
                _mockRegionRepo.Object);
        }

        [TestMethod]
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

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("US", result[0].CountryId);
            Assert.AreEqual("Americas", result[0].RegionName);
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
        [TestMethod]
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

            bool thrown = false;
            try { _service!.CreateCountry(dto); }
            catch (DuplicateException) { thrown = true; }
            Assert.IsTrue(thrown, "DuplicateException should be thrown");
        }
    }
}