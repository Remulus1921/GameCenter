using FakeItEasy;
using FluentAssertions;
using GameCenter.Controllers;
using GameCenter.Core.Services.PlatformService;
using GameCenter.Dtos.PlatformDto;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.Tests.Controller
{
    public class PlatformsControllerTests
    {
        private readonly IPlatformsService _platformsService;

        public PlatformsControllerTests()
        {
            _platformsService = A.Fake<IPlatformsService>();
        }

        [Fact]
        public async void PlatformsController_GetPlatforms_ResultOk()
        {
            //Arrange
            var platforms = A.Fake<List<PlatformDto>>();
            A.CallTo(() => _platformsService.GetPlatforms()).Returns(platforms);
            var controller = new PlatformsController(_platformsService);

            //Act
            var result = await controller.GetPlatforms();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void PlatformsController_AddPlatform_ResultOk()
        {
            //Arrange
            var platform = A.Fake<PlatformDto>();
            A.CallTo(() => _platformsService.AddPlatform(platform)).Returns(true);
            var controller = new PlatformsController(_platformsService);

            //Act
            var result = await controller.AddPlatform(platform);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async void PlatformsController_DeletePlatform_ResultNoContent()
        {
            //Arrqange
            var platform = A.Fake<PlatformDto>();
            A.CallTo(() => _platformsService.DeletePlatform(platform)).Returns(true);
            var controller = new PlatformsController(_platformsService);

            //Act
            var result = await controller.DeletePlatform(platform);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async void PlatformsController_UpdatePlatform_ResultOk()
        {
            //Arrange
            string newName = "";
            var platform = A.Fake<PlatformDto>();
            A.CallTo(() => _platformsService.UpdatePlatform(platform)).Returns(true);
            var controller = new PlatformsController(_platformsService);

            //Act
            var result = await controller.UpdatePlatform(platform);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();
        }
    }
}
