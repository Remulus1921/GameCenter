using GameCenter.Core.Repositories.GenericRepository;
using GameCenter.Data;
using GameCenter.Models;

namespace GameCenter.Core.Repositories.PostsRepository;

public class PostsRepository : GenericRepository<Post>, IPostsRepository
{
    public PostsRepository(ApplicationDbContext context) : base(context)
    {
    }
}
