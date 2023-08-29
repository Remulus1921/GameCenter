using GameCenter.Dtos.CommentDto;

namespace GameCenter.Core.Services.CommentsService;

public interface ICommentService
{
    Task<List<CommentDto>?> GetGameComments(Guid gameId);
    Task<bool?> AddComment(Guid gameId, string email, CommentSmallDto comment);
}
