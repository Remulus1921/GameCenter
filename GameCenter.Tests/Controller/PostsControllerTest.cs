using FakeItEasy;
using FluentAssertions;
using GameCenter.Controllers;
using GameCenter.Core.Services.PostsService;
using GameCenter.Dtos.PostDto;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.Tests.Controller
{
    public class PostsControllerTest
    {
        private readonly IPostsService _postsService;
        public PostsControllerTest()
        {
            _postsService = A.Fake<IPostsService>();
        }

        [Fact]
        public async void PostsController_GetPosts_ReturnOk()
        {
            //Arrange
            var posts = A.Fake<List<PostSmallDto>>();
            A.CallTo(() => _postsService.GetPosts()).Returns(posts);
            var controller = new PostsController(_postsService);

            //Act
            var result = await controller.GetPosts();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void PostsController_GetPosts_ReturnNotFound()
        {
            //Arrange
            A.CallTo(() => _postsService.GetPosts()).Returns(null as List<PostSmallDto>);
            var controller = new PostsController(_postsService);

            //Act
            var result = await controller.GetPosts();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async void PostsController_GetPostById_ReturnOk()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var post = A.Fake<PostDto>();
            A.CallTo(() => _postsService.GetPost(id)).Returns(post);
            var controller = new PostsController(_postsService);

            //Act
            var result = await controller.GetPostById(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void PostsController_GetPostById_ReturnNotFound()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            A.CallTo(() => _postsService.GetPost(id)).Returns(null as PostDto);
            var controller = new PostsController(_postsService);

            //Act
            var result = await controller.GetPostById(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async void PostsController_RemovePost_ReturnNoContent()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            A.CallTo(() => _postsService.RemovePost(id)).Returns(true);
            var controller = new PostsController(_postsService);
            //Act
            var result = await controller.RemovePost(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NoContentResult));
        }

        [Fact]
        public async void PostsController_RemovePost_ReturnNotFounf()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            A.CallTo(() => _postsService.RemovePost(id)).Returns(false);
            var controller = new PostsController(_postsService);
            //Act
            var result = await controller.RemovePost(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async void PostsController_UpdatePost_ReturnOk()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var post = A.Fake<PostAddUpdateDto>();
            A.CallTo(() => _postsService.UpdatePost(id, post)).Returns(true);
            var controller = new PostsController(_postsService);

            //Act
            var result = await controller.UpdatePost(id, post);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void PostsController_UpdatePost_ReturnBadRequest()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var post = A.Fake<PostAddUpdateDto>();
            A.CallTo(() => _postsService.UpdatePost(id, post)).Returns(false);
            var controller = new PostsController(_postsService);

            //Act
            var result = await controller.UpdatePost(id, post);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
