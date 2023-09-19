using GameCenter.Core.Services.CommentsService;
using GameCenter.Dtos.CommentDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCenter.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentService;

        public CommentsController(ICommentsService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{gameId}")]
        public async Task<IActionResult> GameComments([FromRoute] Guid gameId)
        {
            var result = await _commentService.GetGameComments(gameId);

            if (result == null)
            {
                return NotFound("Game Not yet been commented");
            }

            return Ok(result);
        }

        [HttpPost("{gameId}")]
        public async Task<IActionResult> AddComment([FromRoute] Guid gameId, [FromBody] CommentSmallDto comment)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailClaim == null)
            {
                return NotFound("No email claim in authorization");
            }
            string email = emailClaim.Value;

            var result = await _commentService.AddComment(gameId, email, comment);

            return Ok("Comment Successfullt added");
        }

        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateComment([FromRoute] Guid commentId, [FromBody] CommentSmallDto comment)
        {
            var result = await _commentService.UpdateComment(commentId, comment);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid commentId)
        {
            var result = await _commentService.DeleteComment(commentId);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
