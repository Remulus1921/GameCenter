using GameCenter.Core.Repositories.GameRepository;
using GameCenter.Core.Repositories.PlatformRepository;

namespace GameCenter.Data.UnitOfWork;

public interface IUnitOfWork
{
    IPlatformsRepository Platforms { get; }
    IGamesRepository Games { get; }
    Task CompleteAsync();
}
