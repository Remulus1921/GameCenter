using GameCenter.Data;
using GameCenter.Data.UnitOfWork;
using GameCenter.Dtos.CommentDto;
using GameCenter.Models;
using Microsoft.AspNetCore.Identity;

//TODO Update, Delete

namespace GameCenter.Core.Services.CommentsService;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<GameCenterUser> _userManager;

    public CommentService(IUnitOfWork unitOfWork, UserManager<GameCenterUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<bool?>? AddComment(Guid gameId, string email, CommentSmallDto comment)
    {
        var game = await _unitOfWork.Games.GetById(gameId);
        if (game == null)
        {
            return null;
        }
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        var parent = (comment.ParentId == null) ? null : await _unitOfWork.Comments.GetById((Guid)comment.ParentId);

        var newComment = new Comment
        {
            CommentContent = comment.CommentContent,
            ModificationDate = DateTime.Now,
            Game = game,
            User = user,
            Parent = parent,
        };

        await _unitOfWork.Comments.Add(newComment);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<List<CommentDto>?> GetGameComments(Guid gameId)
    {
        var commentList = await _unitOfWork.Comments.GetByGame(gameId);

        if (commentList == null)
        {
            return null;
        }

        List<CommentDto> commentDtoList = new List<CommentDto>();

        foreach (var comment in commentList)
        {
            commentDtoList.Add(new CommentDto
            {
                CommentContent = comment.CommentContent,
                CreationDate = comment.CreationDate,
                ModificationDate = comment.ModificationDate,
                UserName = comment.User.UserName!,
                ParentId = comment.ParentId,
            });
        }

        return commentDtoList;
    }
}
