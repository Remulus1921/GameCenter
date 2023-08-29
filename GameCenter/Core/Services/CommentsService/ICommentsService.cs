using GameCenter.Dtos.CommentDto;

namespace GameCenter.Core.Services.CommentsService;

public interface ICommentsService
{
    Task<List<CommentDto>?> GetGameComments(Guid gameId);
    Task<bool?> AddComment(Guid gameId, string email, CommentSmallDto comment);
    Task<bool> DeleteComment(Guid commentId);
    Task<bool> UpdateComment(Guid commentId, CommentSmallDto comment);
}
