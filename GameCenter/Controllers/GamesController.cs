using GameCenter.Core.Services.GameService;
using GameCenter.Dtos.GameDto;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.Controllers;

// TODO POST, PUT

[Route("[controller]")]
[ApiController]
public class GamesController : ControllerBase
{
    private readonly IGamesService _gamesService;

    public GamesController(IGamesService gamesService)
    {
        _gamesService = gamesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetGames()
    {
        var result = await _gamesService.GetGames();

        if (result == null)
        {
            return NoContent();
        }

        return Ok(result);
    }

    [HttpGet("/{gameId}")]
    public async Task<IActionResult> GetGameById([FromRoute] Guid gameId)
    {
        var result = await _gamesService.GetGameById(gameId);
        if (result == null)
        {
            return BadRequest("There is no game witch id = " + gameId);
        }

        return Ok(result);
    }

    [HttpDelete("/{gameId}")]
    public async Task<IActionResult> DeleteGame([FromRoute] Guid gameId)
    {
        var result = await _gamesService.DeleteGame(gameId);
        if (result == false)
        {
            return NotFound();
        }

        return Ok("Game successfully deleted");
    }

    [HttpPost("addGame")]
    public async Task<IActionResult> AddGame([FromBody] GameAddDto game)
    {
        var result = await _gamesService.AddGame(game);
        return Ok();
    }
}
