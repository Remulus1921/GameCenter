using GameCenter.Core.Services.RatesService;
using GameCenter.Dtos.RateDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCenter.Controllers;

[Authorize]
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

    [HttpGet("userRate/{gameId}")]
    public async Task<IActionResult> GetUserRate([FromRoute] Guid gameId)
    {
        var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        if (emailClaim == null)
        {
            return NotFound("No email claim in authorization");
        }
        string email = emailClaim.Value;
        var result = await _ratesService.GetUserRate(email, gameId);

        if (result == null)
        {
            return BadRequest("No user with given email");
        }

        return Ok(result);
    }

    [HttpPost("addRate/{gameId}")]
    public async Task<IActionResult> AddRate([FromRoute] Guid gameId, [FromBody] RateSmallDto rate)
    {
        if (rate.GameRate == null)
        {
            return BadRequest("Rate can't be null");
        }

        var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        if (emailClaim == null)
        {
            return NotFound("No email claim in authorization");
        }
        string email = emailClaim.Value;

        var (status, message) = await _ratesService.AddRate(rate, gameId, email);

        if (status == 0)
        {
            return NotFound(message);
        }
        else if (status == -1 || status == -2)
        {
            return BadRequest(message);
        }

        return Ok(message);
    }

    [HttpPut("updateRate/{gameId}")]
    public async Task<IActionResult> UpdateRate([FromRoute] Guid gameId, [FromBody] RateSmallDto rate)
    {
        if (rate.GameRate == null)
        {
            return BadRequest("Rate can't be null");
        }

        var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        if (emailClaim == null)
        {
            return NotFound("No email claim in authorization");
        }
        string email = emailClaim.Value;

        var (status, message) = await _ratesService.UpdateRate(rate, gameId, email);

        if (status == 0)
        {
            return NotFound(message);
        }
        else if (status == -1)
        {
            return BadRequest(message);
        }

        return Ok(message);
    }

    [HttpDelete("removeRate/{gameId}")]
    public async Task<IActionResult> RemoveRate([FromRoute] Guid gameId)
    {
        var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        if (emailClaim == null)
        {
            return NotFound("No email claim in authorization");
        }
        string email = emailClaim.Value;

        var (status, message) = await _ratesService.RemoveRate(gameId, email);

        if (status == 0)
        {
            return NotFound(message);
        }
        else if (status == -1)
        {
            return BadRequest(message);
        }

        return Ok(message);
    }
}
