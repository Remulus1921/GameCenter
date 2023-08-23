using GameCenter.Dtos.RateDto;

namespace GameCenter.Core.Services.RatesService;

public interface IRatesService
{
    Task<RateDto?> GetAvarageRate(Guid guid);
    Task<int?> GetUserRate(string email);
}
