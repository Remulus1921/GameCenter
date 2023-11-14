using FakeItEasy;
using FluentAssertions;
using GameCenter.Controllers;
using GameCenter.Core.Services.CommentsService;
using GameCenter.Dtos.CommentDtos;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.Tests.Controller
{
    public class CommentsControllerTests
    {
        private readonly ICommentsService _commentsService;

        public CommentsControllerTests()
        {
            _commentsService = A.Fake<ICommentsService>();
        }

        [Fact]
        public async void CommentsController_GameComments_ResultOk()
        {
            //Arrange
            Guid gameId = Guid.NewGuid();
            var comments = A.Fake<List<CommentDto>>();
            A.CallTo(() => _commentsService.GetGameComments(gameId)).Returns(comments);
            var controller = new CommentsController(_commentsService);

            //Act
            var result = await controller.GameComments(gameId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void CommentsController_UpdateComment_ResultOk()
        {
            //Arrange
            var commentId = Guid.NewGuid();
            var comment = A.Fake<CommentSmallDto>();
            A.CallTo(() => _commentsService.UpdateComment(commentId, comment)).Returns(true);
            var controller = new CommentsController(_commentsService);

            //Act
            var result = await controller.UpdateComment(commentId, comment);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async void CommentsController_DeleteCOmment_ResultNoContent()
        {
            //Arrange
            var commentId = Guid.NewGuid();
            A.CallTo(() => _commentsService.DeleteComment(commentId)).Returns(true);
            var controller = new CommentsController(_commentsService);

            //Act
            var result = await controller.DeleteComment(commentId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
