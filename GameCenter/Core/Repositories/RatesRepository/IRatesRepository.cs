using GameCenter.Core.Repositories.GenericRepository;
using GameCenter.Models;

namespace GameCenter.Core.Repositories.RatesRepository
{

    public interface IRatesRepository : IGenericRepository<Rate>
    {
        Task<Rate?> GetUserRate(string userId, Guid gameId);
    }
}