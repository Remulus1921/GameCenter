using GameCenter.Core.Repositories.GameRepository;
using GameCenter.Core.Repositories.PlatformRepository;
using GameCenter.Core.Repositories.RatesRepository;

namespace GameCenter.Data.UnitOfWork;

public interface IUnitOfWork
{
    IPlatformsRepository Platforms { get; }
    IGamesRepository Games { get; }
    IRatesRepository Rates { get; }
    Task CompleteAsync();
}
