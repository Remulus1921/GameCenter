using GameCenter.Dtos.RateDto;

namespace GameCenter.Core.Services.RatesService;

public interface IRatesService
{
    Task<RateDto?> GetAvarageRate(Guid gameId);
    Task<RateSmallDto?> GetUserRate(string email, Guid gameId);
    Task<(int, string)> AddRate(RateSmallDto rate, Guid gameId, string email);
    Task<(int, string)> UpdateRate(RateSmallDto rate, Guid gameId, string email);
    Task<(int, string)> RemoveRate(Guid gameId, string email);
}
