using FakeItEasy;
using FluentAssertions;
using GameCenter.Controllers;
using GameCenter.Core.Services.GameService;
using GameCenter.Dtos.GameDto;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.Tests.Controller
{
    public class GamesControllerTests
    {
        private readonly IGamesService _gamesService;
        public GamesControllerTests()
        {
            _gamesService = A.Fake<IGamesService>();
        }

        [Fact]
        public async void GamesController_GetGames_ReturnOk()
        {
            //Arrange
            var games = A.Fake<List<GameSmallDto>>();
            A.CallTo(() => _gamesService.GetGames()).Returns(games);
            var controller = new GamesController(_gamesService);

            //Act
            var result = await controller.GetGames();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GamesController_GetGameById_ResultOk()
        {
            //Arrange
            var gameId = Guid.NewGuid();
            var game = A.Fake<GameDto>();
            A.CallTo(() => _gamesService.GetGameById(gameId)).Returns(game);
            var controller = new GamesController(_gamesService);

            //Act
            var result = await controller.GetGameById(gameId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GameController_DeleteGame_ResultNoContent()
        {
            //Arrange
            var gameId = Guid.NewGuid();
            A.CallTo(() => _gamesService.DeleteGame(gameId)).Returns(true);
            var controller = new GamesController(_gamesService);

            //Act
            var result = await controller.DeleteGame(gameId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async void GameController_AddGame_ResultOk()
        {
            //Arrange
            var game = A.Fake<GameAddUpdateDto>();
            A.CallTo(() => _gamesService.AddGame(game)).Returns(true);
            var controller = new GamesController(_gamesService);

            //Act
            var result = await controller.AddGame(game);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GamesController_UpdateGame_ResultOk()
        {
            //Arrange
            var game = A.Fake<GameAddUpdateDto>();
            var gameId = Guid.NewGuid();
            A.CallTo(() => _gamesService.UpdateGame(game, gameId)).Returns(true);
            var controller = new GamesController(_gamesService);

            //Act
            var result = await controller.UpdateGame(gameId, game);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();
        }
    }
}
