using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using HRManagement.API.Models;
using HRManagement.API.DTOs;
using HRManagement.API.Repository;
using HRManagement.API.Services;
using HRManagement.API.Exceptions;

namespace HR.Test
{
    [TestClass]
    public class RegionServiceTests
    {
        private Mock<IRegionRepository>? _mockRepo;
        private RegionService? _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IRegionRepository>();
            _service = new RegionService(_mockRepo.Object);
        }

        [TestMethod]
        public void GetAllRegions_WhenRegionsExist_ReturnsTwoRegions()
        {
            var fakeData = new List<Region>
            {
                new Region { RegionId = 1, RegionName = "Europe" },
                new Region { RegionId = 2, RegionName = "Americas" }
            };
            _mockRepo!.Setup(r => r.GetAll()).Returns(fakeData);

            var result = _service!.GetAllRegions().ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Europe", result[0].RegionName);
        }

        [TestMethod]
        public void CreateRegion_WithValidName_CallsAddAndSave()
        {
            var dto = new CreateRegionDto { RegionName = "South Asia" };
            _mockRepo!.Setup(r => r.GetAll()).Returns(new List<Region>());

            _service!.CreateRegion(dto);

            _mockRepo.Verify(r => r.Add(It.IsAny<Region>()), Times.Once);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CreateRegion_WhenDuplicate_ThrowsDuplicateException()
        {
            var dto = new CreateRegionDto { RegionName = "Europe" };
            _mockRepo!.Setup(r => r.GetAll()).Returns(new List<Region>
            {
                new Region { RegionId = 1, RegionName = "Europe" }
            });

            bool exceptionThrown = false;
            try
            {
                _service!.CreateRegion(dto);
            }
            catch (DuplicateException)
            {
                exceptionThrown = true;
            }
            Assert.IsTrue(exceptionThrown, "DuplicateException should have been thrown");
        }

        [TestMethod]
        public void UpdateRegion_WhenNotFound_ThrowsNotFoundException()
        {
            var dto = new CreateRegionDto { RegionName = "New Name" };
            _mockRepo!.Setup(r => r.GetById((decimal)99)).Returns((Region?)null);

            bool exceptionThrown = false;
            try
            {
                _service!.UpdateRegion((decimal)99, dto);
            }
            catch (NotFoundException)
            {
                exceptionThrown = true;
            }
            Assert.IsTrue(exceptionThrown, "NotFoundException should have been thrown");
        }
    }
}