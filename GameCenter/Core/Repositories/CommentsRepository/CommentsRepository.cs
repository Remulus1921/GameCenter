using GameCenter.Core.Repositories.GenericRepository;
using GameCenter.Data;
using GameCenter.Models;

namespace GameCenter.Core.Repositories.CommentsRepository
{

    public class CommentsRepository : GenericRepository<Comment>, ICommentsRepository
    {
        public CommentsRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Comment>?> GetByGame(Guid gameId)
        {
            return _context.Comments.Where(c => c.GameId == gameId).ToList();
        }
    }
}
