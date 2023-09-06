using GameCenter.Core.Repositories.CommentsRepository;
using GameCenter.Core.Repositories.GameRepository;
using GameCenter.Core.Repositories.PlatformRepository;
using GameCenter.Core.Repositories.PostsRepository;
using GameCenter.Core.Repositories.RatesRepository;

namespace GameCenter.Data.UnitOfWork
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IPlatformsRepository Platforms { get; private set; }
        public IGamesRepository Games { get; private set; }
        public IRatesRepository Rates { get; private set; }
        public ICommentsRepository Comments { get; private set; }
        public IPostsRepository Posts { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Platforms = new PlatformsRepository(_context);
            Games = new GamesRepository(_context);
            Rates = new RatesRepository(_context);
            Comments = new CommentsRepository(_context);
            Posts = new PostsRepository(_context);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
