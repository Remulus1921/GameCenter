using GameCenter.Core.Repositories.GenericRepository;
using GameCenter.Data;
using GameCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCenter.Core.Repositories.CommentsRepository
{

    public class CommentsRepository : GenericRepository<Comment>, ICommentsRepository
    {
        public CommentsRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Comment>?> GetByGame(Guid gameId)
        {
            return _context.Comments.Include(c => c.User).Where(c => c.GameId == gameId).ToList();
        }

        public async override Task<Comment?> GetById(Guid id)
        {
            return _context.Comments.Include(c => c.Replies).SingleOrDefault(c => c.Id == id);
        }
    }
}
