using GameCenter.Core.Services.AuthService;
using GameCenter.Models.User;
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

                var (status, message) = await _authService.Register(registerModel, UserRoles.Admin);

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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromQuery] string userName)
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
                return BadRequest("There is no refresh token in cookies log in again");

            var (status, message, newRefreshToken) = await _authService.RefreshToken(userName, refreshToken);

            if (status == 0)
                return BadRequest(message);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken!.Expires,
            };

            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            return Ok(message);
        }
    }
}
