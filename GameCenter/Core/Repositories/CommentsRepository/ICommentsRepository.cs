using GameCenter.Core.Repositories.GenericRepository;
using GameCenter.Models;

namespace GameCenter.Core.Repositories.CommentsRepository
{

    public interface ICommentsRepository : IGenericRepository<Comment>
    {
        Task<List<Comment>?> GetByGame(Guid gameId);
    }
}
