using FakeItEasy;
using FluentAssertions;
using GameCenter.Core.Services.PlatformService;
using GameCenter.Data.UnitOfWork;
using GameCenter.Dtos.PlatformDto;
using GameCenter.Models;

namespace GameCenter.Tests.Service
{
    public class PlatformsServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlatformsServiceTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
        }

        [Fact]
        public async void PlatformsService_GetPlatforms_ResultListPlatformDto()
        {
            //Arrange
            IEnumerable<Platform> platforms = A.CollectionOfFake<Platform>(3);
            A.CallTo(() => _unitOfWork.Platforms.All()).Returns(platforms);
            var service = new PlatformsService(_unitOfWork);

            //Act
            var result = await service.GetPlatforms();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<PlatformDto>>();
            result.Should().HaveCount(3);
        }

        [Fact]
        public async void PlatformsService_GetPlatforms_ResultNull()
        {
            //Arrang
            var platforms = A.Fake<List<Platform>>();
            A.CallTo(() => _unitOfWork.Platforms.All()).Returns(platforms);
            var service = new PlatformsService(_unitOfWork);

            //Act
            var result = await service.GetPlatforms();

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async void PlatformsService_AddPlatform_ResultTrue()
        {
            //Arrange
            var platformDto = A.Fake<PlatformDto>();
            A.CallTo(() => _unitOfWork.Platforms.GetByName(platformDto.Name)).Returns(null as Platform);
            var service = new PlatformsService(_unitOfWork);

            //Act
            var result = await service.AddPlatform(platformDto);

            //Assert
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
            result.Should().BeTrue();
        }

        [Fact]
        public async void PlatformsService_DeletePlatform_ResultTrue()
        {
            //Arrange
            var platformDto = A.Fake<PlatformDto>();
            var platform = A.Fake<Platform>();
            A.CallTo(() => _unitOfWork.Platforms.GetByName(platformDto.Name)).Returns(platform);
            A.CallTo(() => _unitOfWork.Platforms.Delete(platform)).Returns(true);
            var service = new PlatformsService(_unitOfWork);

            //Act
            var result = await service.DeletePlatform(platformDto);

            //Assert
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
            result.Should().BeTrue();
        }

        [Fact]
        public async void PlatformsService_UpdatePlatform_ResultTrue()
        {
            //Arrange
            var platformDto = A.Fake<PlatformDto>();
            var platform = A.Fake<Platform>();
            var newName = "Fake Name";
            A.CallTo(() => _unitOfWork.Platforms.GetByName(platformDto.Name)).Returns(platform);
            A.CallTo(() => _unitOfWork.Platforms.Update(platform)).Returns(true);
            var service = new PlatformsService(_unitOfWork);

            //Act
            var result = await service.UpdatePlatform(platformDto);

            //Assert
            result.Should().BeTrue();
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }
    }
}
