using FakeItEasy;
using FluentAssertions;
using GameCenter.Controllers;
using GameCenter.Core.Services.RatesService;
using GameCenter.Dtos.RateDto;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.Tests.Controller
{
    public class RatesControllerTests
    {
        private readonly IRatesService _ratesService;

        public RatesControllerTests()
        {
            _ratesService = A.Fake<IRatesService>();
        }

        [Fact]
        public async void RatesController_GetAvarageRate_ResultOk()
        {
            //Arrange
            Guid gameId = Guid.NewGuid();
            var rate = A.Fake<RateDto>();
            A.CallTo(() => _ratesService.GetAvarageRate(gameId)).Returns(rate);
            var controller = new RatesController(_ratesService);

            //Act
            var result = await controller.GetAvarageRate(gameId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
