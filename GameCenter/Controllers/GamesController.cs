using GameCenter.Core.Services.GameService;
using GameCenter.Dtos.GameDto;
using GameCenter.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGamesService _gamesService;

        public GamesController(IGamesService gamesService)
        {
            _gamesService = gamesService;
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet("{gameId}")]
        public async Task<IActionResult> GetGameById([FromRoute] Guid gameId)
        {
            var result = await _gamesService.GetGameById(gameId);
            if (result == null)
            {
                return BadRequest("There is no game witch id = " + gameId);
            }

            return Ok(result);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Moderator)]
        [HttpDelete("{gameId}")]
        public async Task<IActionResult> DeleteGame([FromRoute] Guid gameId)
        {
            var result = await _gamesService.DeleteGame(gameId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Moderator)]
        [HttpPost("addGame")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> AddGame([FromForm] GameAddUpdateDto game, [FromForm] IFormFile image)
        {
            if (image != null)
            {
                game.Image = image;
            }
            else
            {
                return BadRequest("Brak zdjęcia");
            }

            var result = await _gamesService.AddGame(game);
            if (!result)
            {
                return BadRequest("Wystąpił błąd podczas dodawania gry");
            }

            return Ok("Dodano nową grę");
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Moderator)]
        [HttpPut("{gameId}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UpdateGame([FromRoute] Guid gameId, [FromForm] GameAddUpdateDto game, [FromForm] IFormFile image)
        {
            if (image != null)
            {
                game.Image = image;
            }

            var result = await _gamesService.UpdateGame(game, gameId);
            if (!result)
                return NotFound();

            return Ok("Game successfully updated");
        }

    }
}
