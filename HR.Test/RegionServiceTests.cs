using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        private Mock<IMapper>? _mockMapper;
        private RegionService? _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new();
            _mockMapper = new();
            _service = new(
                _mockRepo.Object,
                _mockMapper.Object);
        }


        [TestMethod]
        public void GetAllRegions_WhenRegionsExist_ReturnsTwoRegions()
        {
            var fakeRegions = new List<Region>
            {
                new() { RegionId = 1, RegionName = "Europe" },
                new() { RegionId = 2, RegionName = "Americas" }
            };

            var fakeDtos = new List<RegionDto>
            {
                new() { RegionId = 1, RegionName = "Europe" },
                new() { RegionId = 2, RegionName = "Americas" }
            };

            _mockRepo!
                .Setup(r => r.GetAll())
                .Returns(fakeRegions);

            _mockMapper!
                .Setup(m => m.Map<IEnumerable<RegionDto>>(
                    fakeRegions))
                .Returns(fakeDtos);

            var result = _service!
                .GetAllRegions()
                .ToList();

            Assert.AreEqual(2, result.Count,
                "Should return 2 regions");

            Assert.AreEqual("Europe",
                result[0].RegionName,
                "First region should be Europe");
        }

        [TestMethod]
        public void CreateRegion_WithValidName_CallsAddAndSave()
        {
            var dto = new CreateRegionDto
            {
                RegionName = "South Asia"
            };

            _mockRepo!
                .Setup(r => r.ExistsByName("South Asia"))
                .Returns(false);

            _mockMapper!
                .Setup(m => m.Map<Region>(dto))
                .Returns(new Region
                {
                    RegionName = "South Asia"
                });

            _service!.CreateRegion(dto);

            _mockRepo.Verify(
                r => r.Add(It.IsAny<Region>()),
                Times.Once,
                "Add should be called once");

            _mockRepo.Verify(
                r => r.SaveChanges(),
                Times.Once,
                "SaveChanges should be called once");
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateException))]
        public void CreateRegion_WhenDuplicate_ThrowsDuplicateException()
        {
            var dto = new CreateRegionDto
            {
                RegionName = "Europe"
            };

            _mockRepo!
                .Setup(r => r.ExistsByName("Europe"))
                .Returns(true);

            _service!.CreateRegion(dto);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void UpdateRegion_WhenNotFound_ThrowsNotFoundException()
        {
            var dto = new CreateRegionDto
            {
                RegionName = "New Name"
            };

            _mockRepo!
                .Setup(r => r.GetById(99))
                .Returns((Region?)null);

            _service!.UpdateRegion(99, dto);
        }

    }
}