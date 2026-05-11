using HRManagement.API.DTOs;
using HRManagement.API.Exceptions;
using HRManagement.API.Models;
using HRManagement.API.Repository;
using HRManagement.API.Services;
using Moq;

namespace HR.Test
{
    public class RegionServiceTests
    {
        private Mock<IRegionRepository>? _mockRepo;
        private RegionService? _service;

        public RegionServiceTests()
        {
            _mockRepo = new Mock<IRegionRepository>();
            _service = new RegionService(_mockRepo.Object);
        }

        [Fact]
        public void GetAllRegions_WhenRegionsExist_ReturnsTwoRegions()
        {
            var fakeData = new List<Region>
            {
                new Region { RegionId = 1, RegionName = "Europe" },
                new Region { RegionId = 2, RegionName = "Americas" }
            };
            _mockRepo!.Setup(r => r.GetAll()).Returns(fakeData);

            var result = _service!.GetAllRegions().ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Europe", result[0].RegionName);
        }

        [Fact]
        public void CreateRegion_WithValidName_CallsAddAndSave()
        {
            var dto = new CreateRegionDto { RegionName = "South Asia" };
            _mockRepo!.Setup(r => r.GetAll()).Returns(new List<Region>());

            _service!.CreateRegion(dto);

            _mockRepo.Verify(r => r.Add(It.IsAny<Region>()), Times.Once);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void CreateRegion_WhenDuplicate_ThrowsDuplicateException()
        {
            var dto = new CreateRegionDto { RegionName = "Europe" };
            _mockRepo!.Setup(r => r.GetAll()).Returns(new List<Region>
            {
                new Region { RegionId = 1, RegionName = "Europe" }
            });

            Assert.Throws<DuplicateException>(() => _service!.CreateRegion(dto));
        }

        [Fact]
        public void UpdateRegion_WhenNotFound_ThrowsNotFoundException()
        {
            var dto = new CreateRegionDto { RegionName = "New Name" };
            _mockRepo!.Setup(r => r.GetById((decimal)99)).Returns((Region?)null);

            Assert.Throws<NotFoundException>(() => _service!.UpdateRegion((decimal)99, dto));
        }
    }
}
