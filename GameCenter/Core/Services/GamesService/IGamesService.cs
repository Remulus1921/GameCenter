using GameCenter.Dtos.GameDto;

namespace GameCenter.Core.Services.GameService;

public interface IGamesService
{
    Task<List<GameSmallDto>> GetGames();
    Task<GameDto?> GetGameById(Guid id);
    Task<bool> DeleteGame(Guid id);
    Task<bool> AddGame(GameAddUpdateDto game);
    Task<bool> UpdateGame(GameAddUpdateDto game, Guid gameId);
}
