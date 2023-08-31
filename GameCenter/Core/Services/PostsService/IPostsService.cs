using GameCenter.Dtos.PostDto;

namespace GameCenter.Core.Services.PostsService;

public interface IPostsService
{
    Task<List<PostSmallDto>?> GetPosts();
    Task<PostDto?> GetPost(Guid postId);
    Task<bool> RemovePost(Guid postId);
    Task<bool> UpdatePost(Guid postId, PostAddUpdateDto post);
    Task<bool> AddPost(PostAddUpdateDto post, string email);
}
