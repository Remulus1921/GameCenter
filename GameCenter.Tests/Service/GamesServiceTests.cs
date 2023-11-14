using FakeItEasy;
using GameCenter.Data.UnitOfWork;
using GameCenter.Dtos.GameDto;
using GameCenter.Models;

namespace GameCenter.Tests.Service
{
    public class GamesServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;

        public GamesServiceTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
        }

        [Fact]
        public async void GamesService_AddGame_ResultTrue()
        {
            //Arrange
            var game = A.Fake<GameAddUpdateDto>();
            game.Platforms = A.Fake<List<string>>();
            //var service = new GamesService(_unitOfWork);

            //Act
            //var resut = await service.AddGame(game);

            //Assert
            //resut.Should().BeTrue();
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void GamesService_DeleteGame_ResultTrue()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var game = A.Fake<Game>();
            A.CallTo(() => _unitOfWork.Games.GetById(id)).Returns(game);
            A.CallTo(() => _unitOfWork.Games.Delete(game)).Returns(true);
            //var service = new GamesService(_unitOfWork);

            //Act
            //var result = await service.DeleteGame(id);

            //Assert
            //result.Should().BeTrue();
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void GamesService_GetGameById_ResultGameDto()
        {
            //Arrange
            Guid gameId = Guid.NewGuid();
            var game = A.Fake<Game>();
            game.GameRates = A.Fake<List<Rate>>();
            game.Platforms = A.Fake<List<Platform>>();
            game.GameComments = A.Fake<List<Comment>>();
            A.CallTo(() => _unitOfWork.Games.GetById(gameId)).Returns(game);
            //var service = new GamesService(_unitOfWork);
            //Act
            //var result = await service.GetGameById(gameId);

            //Assert
            //result.Should().NotBeNull();
            //result.Should().BeOfType<GameDto>();
        }

        [Fact]
        public async void GamesService_GetGames_ResultListGameDto()
        {
            //Arrange
            var games = A.Fake<IEnumerable<Game>>();
            A.CallTo(() => _unitOfWork.Games.All()).Returns(games);
            //var services = new GamesService(_unitOfWork);

            //Act
            //var result = await services.GetGames();

            //Assert
            //result.Should().NotBeNull();
            //result.Should().BeOfType<List<GameSmallDto>>();
        }

        [Fact]
        public async void GamesService_UpdateGame_ResultTrue()
        {
            //Arrange
            var game = A.Fake<GameAddUpdateDto>();
            game.Platforms = A.Fake<List<string>>();
            Guid gameId = Guid.NewGuid();
            var gameEx = A.Fake<Game>();
            A.CallTo(() => _unitOfWork.Games.GetById(gameId)).Returns(gameEx);
            A.CallTo(() => _unitOfWork.Games.Update(gameEx)).Returns(true);
            //var services = new GamesService(_unitOfWork);

            //Act
            //var result = await services.UpdateGame(game, gameId);

            //Assert
            //result.Should().BeTrue();
            //A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }
    }
}
