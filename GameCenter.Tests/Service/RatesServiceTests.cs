using FakeItEasy;
using FluentAssertions;
using GameCenter.Core.Services.RatesService;
using GameCenter.Data;
using GameCenter.Data.UnitOfWork;
using GameCenter.Dtos.RateDto;
using GameCenter.Models;
using Microsoft.AspNetCore.Identity;

namespace GameCenter.Tests.Service
{
    public class RatesServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<GameCenterUser> _userManager;

        public RatesServiceTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _userManager = A.Fake<UserManager<GameCenterUser>>();
        }

        [Fact]
        public async void RatesService_AddRate_Result1String()
        {
            //Arrange
            var rateDto = A.Fake<RateSmallDto>();
            rateDto.GameRate = 1;
            Guid gameId = Guid.NewGuid();
            string email = string.Empty;
            var game = A.Fake<Game>();
            var user = A.Fake<GameCenterUser>();
            A.CallTo(() => _unitOfWork.Games.GetById(gameId)).Returns(game);
            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(user);
            A.CallTo(() => _unitOfWork.Rates.GetUserRate(user.Id, game.Id)).Returns(null as Rate);
            var service = new RatesService(_unitOfWork, _userManager);

            //Act
            var result = await service.AddRate(rateDto, gameId, email);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be((1, "Rate Successfully added"));
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void RatesServce_GetAvarageRate_ResultRateDto()
        {
            //Arrange
            Guid gameId = Guid.NewGuid();
            var game = A.Fake<Game>();
            game.GameRates = A.CollectionOfFake<Rate>(3);
            A.CallTo(() => _unitOfWork.Games.GetById(gameId)).Returns(game);
            var service = new RatesService(_unitOfWork, _userManager);

            //Act
            var result = await service.GetAvarageRate(gameId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<RateDto>();
        }

        [Fact]
        public async void RatesService_GetUserRate_ResultRateSmallDto()
        {
            //Arrange
            string email = string.Empty;
            Guid gameId = Guid.NewGuid();
            var game = A.Fake<Game>();
            var user = A.Fake<GameCenterUser>();
            var rate = A.Fake<Rate>();
            A.CallTo(() => _unitOfWork.Games.GetById(gameId)).Returns(game);
            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(user);
            A.CallTo(() => _unitOfWork.Rates.GetUserRate(user.Id, game.Id)).Returns(rate);
            var service = new RatesService(_unitOfWork, _userManager);

            //Act
            var result = await service.GetUserRate(email, gameId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<RateSmallDto>();
        }

        [Fact]
        public async void RatesService_RemoveRate_Result1String()
        {
            //Arrange
            Guid gameId = Guid.NewGuid();
            string email = string.Empty;
            var game = A.Fake<Game>();
            var user = A.Fake<GameCenterUser>();
            var rate = A.Fake<Rate>();
            A.CallTo(() => _unitOfWork.Games.GetById(gameId)).Returns(game);
            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(user);
            A.CallTo(() => _unitOfWork.Rates.GetUserRate(user.Id, game.Id)).Returns(rate);
            A.CallTo(() => _unitOfWork.Rates.Delete(rate)).Returns(true);
            var service = new RatesService(_unitOfWork, _userManager);

            //Act
            var result = await service.RemoveRate(gameId, email);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be((1, "Rate Successfully deleted"));
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void RatesService_UpdateRate_Result1String()
        {
            //Arrange
            var rateDto = A.Fake<RateSmallDto>();
            rateDto.GameRate = 1;
            Guid gameId = Guid.NewGuid();
            string email = string.Empty;
            var game = A.Fake<Game>();
            var user = A.Fake<GameCenterUser>();
            var rate = A.Fake<Rate>();
            A.CallTo(() => _unitOfWork.Games.GetById(gameId)).Returns(game);
            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(user);
            A.CallTo(() => _unitOfWork.Rates.GetUserRate(user.Id, game.Id)).Returns(rate);
            A.CallTo(() => _unitOfWork.Rates.Update(rate)).Returns(true);
            var service = new RatesService(_unitOfWork, _userManager);

            //Act
            var result = await service.UpdateRate(rateDto, gameId, email);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be((1, "Rate Successfully updated"));
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }

    }
}
