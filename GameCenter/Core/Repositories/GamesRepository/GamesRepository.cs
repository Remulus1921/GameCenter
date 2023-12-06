using GameCenter.Core.Repositories.GenericRepository;
using GameCenter.Data;
using GameCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCenter.Core.Repositories.GameRepository
{

    public class GamesRepository : GenericRepository<Game>, IGamesRepository
    {
        public GamesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Game?> GetById(Guid id)
        {
            return await _context.Games
                .Include(g => g.Platforms)
                .Include(g => g.GameRates)
                .Include(g => g.GameComments)
                .ThenInclude(c => (c as Comment).User)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public override async Task<IEnumerable<Game>> All()
        {
            return await _context.Games.Include(g => g.Platforms).ToListAsync();
        }
    }
}
