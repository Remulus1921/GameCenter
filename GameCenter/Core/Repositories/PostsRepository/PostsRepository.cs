using GameCenter.Core.Repositories.GenericRepository;
using GameCenter.Data;
using GameCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCenter.Core.Repositories.PostsRepository
{

    public class PostsRepository : GenericRepository<Post>, IPostsRepository
    {
        public PostsRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Post>> All()
        {
            return await _context.Posts.Include(p => p.User).Include(p => p.Platforms).ToListAsync();
        }

        public override async Task<Post?> GetById(Guid id)
        {
            return await _context.Posts.Include(p => p.User).Include(p => p.Platforms).FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
