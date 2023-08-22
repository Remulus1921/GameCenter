using GameCenter.Core.Repositories.PlatformRepository;

namespace GameCenter.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IPlatformRepository Platforms { get; }
        Task CompleteAsync();
    }
}
