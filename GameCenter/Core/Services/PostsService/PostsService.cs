using GameCenter.Data;
using GameCenter.Data.UnitOfWork;
using GameCenter.Dtos.PostDto;
using GameCenter.Models;
using Microsoft.AspNetCore.Identity;

namespace GameCenter.Core.Services.PostsService;

public class PostsService : IPostsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<GameCenterUser> _userManager;

    public PostsService(IUnitOfWork unitOfWork, UserManager<GameCenterUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<bool> AddPost(PostAddUpdateDto post, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        var platforms = new List<Platform>();
        foreach (var platformName in post.Platforms)
        {
            var platform = await _unitOfWork.Platforms.GetByName(platformName);
            if (platform == null)
                continue;
            platforms.Add(platform);
        }

        var newPost = new Post
        {
            Title = post.Title,
            Content = post.Content,
            Modified = DateTime.Now,
            ImagePath = post.ImagePath,
            User = user,
            Platforms = platforms
        };

        await _unitOfWork.Posts.Add(newPost);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<PostDto?> GetPost(Guid postId)
    {
        var post = await _unitOfWork.Posts.GetById(postId);
        if (post == null)
        {
            return null;
        }

        var postDto = new PostDto
        {
            Title = post.Title,
            Content = post.Content,
            Created = post.Created,
            Modified = post.Modified,
            ImagePath = post.ImagePath,
            UserName = post.User.UserName,
            Platforms = post.Platforms.Select(p => p.PlatformName).ToList(),
        };

        return postDto;
    }

    public async Task<List<PostSmallDto>?> GetPosts()
    {
        var posts = await _unitOfWork.Posts.All();

        if (posts == null)
        {
            return null;
        }

        List<PostSmallDto> postsList = new List<PostSmallDto>();

        foreach (var post in posts)
        {
            postsList.Add(new PostSmallDto
            {
                Id = post.Id,
                Title = post.Title,
                Created = post.Created,
                Modified = post.Modified,
                ImagePath = post.ImagePath,
                UserName = post.User.UserName!,
                Platforms = post.Platforms.Select(p => p.PlatformName).ToList(),
            });
        }

        return postsList;
    }

    public async Task<bool> RemovePost(Guid postId)
    {
        var post = await _unitOfWork.Posts.GetById(postId);
        if (post == null)
        {
            return false;
        }

        await _unitOfWork.Posts.Delete(post);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<bool> UpdatePost(Guid postId, PostAddUpdateDto post)
    {
        var postExists = await _unitOfWork.Posts.GetById(postId);

        if (postExists == null)
        {
            return false;
        }

        var platforms = new List<Platform>();
        foreach (var platformName in post.Platforms)
        {
            var platform = await _unitOfWork.Platforms.GetByName(platformName);
            if (platform == null)
                continue;
            platforms.Add(platform);
        }

        postExists.Title = post.Title;
        postExists.Content = post.Content;
        postExists.Modified = DateTime.Now;
        postExists.Platforms = platforms;
        postExists.ImagePath = post.ImagePath;

        await _unitOfWork.Posts.Update(postExists);
        await _unitOfWork.CompleteAsync();

        return true;
    }
}
