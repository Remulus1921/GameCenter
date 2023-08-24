using GameCenter.Core.Repositories.GenericRepository;
using GameCenter.Data;
using GameCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCenter.Core.Repositories.RatesRepository
{
    public class RatesRepository : GenericRepository<Rate>, IRatesRepository
    {
        public RatesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Rate?> GetUserRate(string userId, Guid gameId)
        {
            return await _context.Rates.Where(r => r.UserId.Equals(userId) && r.GameId == gameId).FirstOrDefaultAsync();
        }
    }
}
