using FakeItEasy;
using FluentAssertions;
using GameCenter.Core.Services.PostsService;
using GameCenter.Data;
using GameCenter.Data.UnitOfWork;
using GameCenter.Dtos.PostDto;
using GameCenter.Models;
using Microsoft.AspNetCore.Identity;

namespace GameCenter.Tests.Service
{
    public class PostsServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<GameCenterUser> _userManager;

        public PostsServiceTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _userManager = A.Fake<UserManager<GameCenterUser>>();
        }

        [Fact]
        public async void PostsService_AddPost_ResultTrue()
        {
            //Arrange
            var postDto = A.Fake<PostAddUpdateDto>();
            var user = A.Fake<GameCenterUser>();
            var email = string.Empty;
            postDto.Platforms = new List<string>();
            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(user);
            var service = new PostsService(_unitOfWork, _userManager);

            //Act
            var result = await service.AddPost(postDto, email);

            //Assert
            result.Should().BeTrue();
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void PostsService_GetPost_ResultPostDto()
        {
            //Arrange
            Guid postId = Guid.NewGuid();
            var post = A.Fake<Post>();
            post.Platforms = A.Fake<List<Platform>>();
            post.User = A.Fake<GameCenterUser>();
            post.User.UserName = "Test";
            A.CallTo(() => _unitOfWork.Posts.GetById(postId)).Returns(post);
            var service = new PostsService(_unitOfWork, _userManager);

            //Act
            var result = await service.GetPost(postId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<PostDto>();
        }

        [Fact]
        public async void PostsService_GetPosts_ResultListPostSmallDto()
        {
            //Arrange
            var posts = new List<Post>();
            A.CallTo(() => _unitOfWork.Posts.All()).Returns(posts);
            var service = new PostsService(_unitOfWork, _userManager);

            //Act
            var result = await service.GetPosts();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<PostSmallDto>>();
        }

        [Fact]
        public async void PostsService_RemovePost_ResultTrue()
        {
            //Arrange
            Guid postId = Guid.NewGuid();
            var post = A.Fake<Post>();
            A.CallTo(() => _unitOfWork.Posts.GetById(postId)).Returns(post);
            A.CallTo(() => _unitOfWork.Posts.Delete(post));
            var service = new PostsService(_unitOfWork, _userManager);

            //Act
            var result = await service.RemovePost(postId);

            //Assert
            result.Should().BeTrue();
            result.Should().NotBe(false);
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void PostsService_UpdatePost_ResultTrue()
        {
            //Arrange
            Guid postId = Guid.NewGuid();
            var postDto = A.Fake<PostAddUpdateDto>();
            postDto.Platforms = A.Fake<List<string>>();
            var post = A.Fake<Post>();
            A.CallTo(() => _unitOfWork.Posts.GetById(postId)).Returns(post);
            A.CallTo(() => _unitOfWork.Posts.Update(post)).Returns(true);
            var service = new PostsService(_unitOfWork, _userManager);

            //Act
            var result = await service.UpdatePost(postId, postDto);

            //Assert
            result.Should().BeTrue();
            result.Should().NotBe(false);
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }
    }
}
