using GameCenter.Core.Services.AuthService;
using GameCenter.Dtos.UserDtos;
using GameCenter.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");

                var (status, message, refreshToken) = await _authService.Login(model);

                if (status == 0)
                    return BadRequest(message);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = refreshToken!.Expires,
                    Secure = true
                };

                Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

                return Ok(new { token = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid Payload!");

                var (status, message) = await _authService.Register(registerModel, UserRoles.User);

                if (status == 0)
                    return BadRequest(message);

                return CreatedAtAction(nameof(Register), registerModel);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("refresh-token/{email}")]
        public async Task<IActionResult> RefreshToken([FromRoute] string email)
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
                return BadRequest("Zaloguj się");

            var (status, message, newRefreshToken) = await _authService.RefreshToken(email, refreshToken);

            if (status == 0)
                return BadRequest(message);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken!.Expires,
                Secure = true,
            };

            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            return Ok(new { token = message });
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Moderator)]
        [HttpGet("get-users")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _authService.GetUsersList();
            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("user-details/{userEmail}")]
        public async Task<IActionResult> GetUserDetails([FromRoute] string userEmail)
        {
            if (userEmail == null)
                return BadRequest();

            var result = await _authService.GetUserDetails(userEmail);
            if (result == null)
                return BadRequest();

            return Ok(result);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Moderator)]
        [HttpPut("grant-the-role")]
        public async Task<IActionResult> UpdateUserRole([FromBody] RoleDto roleDto)
        {
            if (roleDto == null)
                return BadRequest("Brak danych odnośnie użytkownika lub rangi");

            var result = await _authService.UpdateUserRole(roleDto.RoleName, roleDto.UserEmail);

            if (result == false)
                return BadRequest("Błąd dodawania do rangi");

            return Ok();
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Moderator)]
        [HttpDelete("delete-user/{userEmail}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string userEmail)
        {
            var result = await _authService.DeleteUserByEmail(userEmail);

            if (result == null)
                return BadRequest("Błąd podczas usuwania użytkownika");
            else if (result == false)
                return BadRequest("Błąd podczas usuwania użytkownika");

            return NoContent();
        }
    }
}
