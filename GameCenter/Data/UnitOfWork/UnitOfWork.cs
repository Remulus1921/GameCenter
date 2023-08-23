using GameCenter.Core.Repositories.GameRepository;
using GameCenter.Core.Repositories.PlatformRepository;

namespace GameCenter.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IPlatformsRepository Platforms { get; private set; }
    public IGamesRepository Games { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Platforms = new PlatformsRepository(_context);
        Games = new GamesRepository(_context);
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}
