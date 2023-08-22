using GameCenter.Core.Repositories.GenericRepository;
using GameCenter.Models;

namespace GameCenter.Core.Repositories.PlatformRepository;

public interface IPlatformRepository : IGenericRepository<Platform>
{
    Task<Platform?> GetByName(string name);
}
