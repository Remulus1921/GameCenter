using FakeItEasy;
using FluentAssertions;
using GameCenter.Core.Services.CommentsService;
using GameCenter.Data;
using GameCenter.Data.UnitOfWork;
using GameCenter.Dtos.CommentDto;
using GameCenter.Models;
using Microsoft.AspNetCore.Identity;

namespace GameCenter.Tests.Service
{
    public class CommentsServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<GameCenterUser> _userManager;

        public CommentsServiceTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _userManager = A.Fake<UserManager<GameCenterUser>>();
        }

        [Fact]
        public async void CommentsService_AddComment_ResultTrue()
        {
            //Arrange
            Guid gameId = Guid.NewGuid();
            string email = string.Empty;
            var game = A.Fake<Game>();
            var user = A.Fake<GameCenterUser>();
            var comment = A.Fake<CommentSmallDto>();
            A.CallTo(() => _unitOfWork.Games.GetById(gameId)).Returns(game);
            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(user);
            A.CallTo(() => _unitOfWork.CompleteAsync());
            var service = new CommentsService(_unitOfWork, _userManager);

            //Act
            var result = await service.AddComment(gameId, email, comment);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be(true);
        }

        [Fact]
        public async void CommentsService_DeleteComment_ResultTrue()
        {
            //Arrange
            Guid commentId = Guid.NewGuid();
            var comment = A.Fake<Comment>();
            A.CallTo(() => _unitOfWork.Comments.GetById(commentId)).Returns(comment);
            A.CallTo(() => _unitOfWork.Comments.Delete(comment)).Returns(true);
            A.CallTo(() => _unitOfWork.CompleteAsync());
            var service = new CommentsService(_unitOfWork, _userManager);

            //Act
            var result = await service.DeleteComment(commentId);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async void CommentsService_GetGameComments_ResultListCommentDto()
        {
            //Arrange
            Guid gameId = Guid.NewGuid();
            var commentList = A.Fake<List<Comment>>();
            A.CallTo(() => _unitOfWork.Comments.GetByGame(gameId)).Returns(commentList);
            var service = new CommentsService(_unitOfWork, _userManager);

            //Act
            var result = await service.GetGameComments(gameId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<CommentDto>>();
        }

        [Fact]
        public async void CommentsService_UpdateComment_ResultTrue()
        {
            //Arrange
            Guid commentId = Guid.NewGuid();
            var comment = A.Fake<CommentSmallDto>();
            var commentExists = A.Fake<Comment>();
            A.CallTo(() => _unitOfWork.Comments.GetById(commentId)).Returns(commentExists);
            A.CallTo(() => _unitOfWork.CompleteAsync());
            var service = new CommentsService(_unitOfWork, _userManager);

            //Act
            var result = await service.UpdateComment(commentId, comment);

            //Assert
            result.Should().BeTrue();
        }
    }
}
