using GameCenter.Core.Services.PostsService;
using GameCenter.Dtos.PostDto;
using GameCenter.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCenter.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostsService _postsService;

        public PostsController(IPostsService postsService)
        {
            _postsService = postsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var result = await _postsService.GetPosts();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById([FromRoute] Guid postId)
        {
            var result = await _postsService.GetPost(postId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Moderator)]
        [HttpDelete("{postId}")]
        public async Task<IActionResult> RemovePost([FromRoute] Guid postId)
        {
            var result = await _postsService.RemovePost(postId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Moderator)]
        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid postId, [FromBody] PostAddUpdateDto post)
        {
            var result = await _postsService.UpdatePost(postId, post);

            if (!result)
            {
                return BadRequest();
            }

            return Ok("Post Updated Successfully");
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Moderator)]
        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] PostAddUpdateDto post)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailClaim == null)
            {
                return NotFound("No email claim in authorization");
            }
            string email = emailClaim.Value;

            var result = await _postsService.AddPost(post, email);

            if (!result)
            {
                return NotFound("No User with provided email");
            }

            return Ok("Post Successfully added");
        }
    }
}
