using GameCenter.Core.Services.CommentsService;
using GameCenter.Dtos.CommentDto;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{gameId}")]
        public async Task<IActionResult> GameComments([FromRoute] Guid gameId)
        {
            var result = _commentService.GetGameComments(gameId);

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
    }
}
