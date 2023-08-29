using GameCenter.Data.UnitOfWork;
using GameCenter.Dtos.CommentDto;
using GameCenter.Dtos.GameDto;
using GameCenter.Models;

namespace GameCenter.Core.Services.GameService;

public class GamesService : IGamesService
{
    private readonly IUnitOfWork _unitOfWork;

    public GamesService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> AddGame(GameAddUpdateDto game)
    {
        var gamePlatforms = new List<Platform>();

        foreach (var platformName in game.Platforms)
        {
            var platform = await _unitOfWork.Platforms.GetByName(platformName);
            if (platform == null)
                continue;
            gamePlatforms.Add(platform);
        }

        Game newGame = new Game
        {
            Name = game.Name,
            GameType = game.GameType,
            Description = game.Description,
            Studio = game.Studio,
            Rating = game.Rating,
            Capacity = game.Capacity,
            ImagePath = game.ImagePath,
            Platforms = gamePlatforms,
        };

        await _unitOfWork.Games.Add(newGame);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<bool> DeleteGame(Guid id)
    {
        var game = await _unitOfWork.Games.GetById(id);
        if (game == null)
        {
            return false;
        }

        await _unitOfWork.Games.Delete(game);
        await _unitOfWork.CompleteAsync();

        return true;
    }


    public async Task<GameDto?> GetGameById(Guid id)
    {
        var game = await _unitOfWork.Games.GetById(id);
        if (game == null)
        {
            return null;
        }

        var gameDto = new GameDto
        {
            Id = game.Id,
            Name = game.Name,
            GameType = game.GameType,
            Description = game.Description,
            Studio = game.Studio,
            Capacity = game.Capacity,
            ImagePath = game.ImagePath,
            PlatformsName = game.Platforms.Select(p => p.PlatformName).ToList(),
            GameRates = game.GameRates.Select(r => r.GameRate).ToList(),
            Comments = game.GameComments.Select(c => new CommentSmallDto
            {
                CommentContent = c.CommentContent,
                ParentId = c.ParentId
            }).ToList(),
        };

        return gameDto;
    }

    public async Task<List<GameSmallDto>> GetGames()
    {
        var games = await _unitOfWork.Games.All();
        var gamesDtoList = new List<GameSmallDto>();

        foreach (var game in games)
        {
            gamesDtoList.Add(new GameSmallDto
            {
                Id = game.Id,
                Name = game.Name,
                GameType = game.GameType,
                Rating = game.Rating,
                ImagePath = game.ImagePath,
            });
        }

        return gamesDtoList;
    }

    public async Task<bool> UpdateGame(GameAddUpdateDto game, Guid gameId)
    {
        var gameExists = await _unitOfWork.Games.GetById(gameId);

        if (gameExists == null)
        {
            return false;
        }

        var gamePlatforms = new List<Platform>();

        foreach (var platformName in game.Platforms)
        {
            var platform = await _unitOfWork.Platforms.GetByName(platformName);
            if (platform == null)
                continue;
            gamePlatforms.Add(platform);
        }

        gameExists.Name = game.Name;
        gameExists.GameType = game.GameType;
        gameExists.Rating = game.Rating;
        gameExists.Description = game.Description;
        gameExists.Studio = game.Studio;
        gameExists.Capacity = game.Capacity;
        gameExists.ImagePath = game.ImagePath;
        gameExists.Platforms = gamePlatforms;

        await _unitOfWork.Games.Update(gameExists);
        await _unitOfWork.CompleteAsync();

        return true;
    }
}
