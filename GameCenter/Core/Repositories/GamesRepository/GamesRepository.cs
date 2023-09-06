using GameCenter.Core.Repositories.GenericRepository;
using GameCenter.Data;
using GameCenter.Models;

namespace GameCenter.Core.Repositories.GameRepository
{

    public class GamesRepository : GenericRepository<Game>, IGamesRepository
    {
        public GamesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
