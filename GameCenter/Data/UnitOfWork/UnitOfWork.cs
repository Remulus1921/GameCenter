using GameCenter.Core.Repositories.PlatformRepository;

namespace GameCenter.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IPlatformRepository Platforms { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Platforms = new PlatformRepository(_context);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
