using GameCenter.Core.Services.RatesService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCenter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        private readonly IRatesService _ratesService;

        public RatesController(IRatesService ratesService)
        {
            _ratesService = ratesService;
        }

        [HttpGet("gameRate/{gameId}")]
        public async Task<IActionResult> GetAvarageRate([FromRoute] Guid gameId)
        {
            var result = await _ratesService.GetAvarageRate(gameId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize]
        [HttpGet("/userRate")]
        public async Task<IActionResult> GetUserRate()
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailClaim == null)
            {
                return NotFound("No email claim in authorization");
            }
            string email = emailClaim.Value;
            var result = await _ratesService.GetUserRate(email);

            if (result == null)
            {
                return BadRequest("No user with given email");
            }

            return Ok(result);
        }

    }
}
