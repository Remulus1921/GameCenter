using GameCenter.Models.User;
using GameCenter.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthenticateController> _logger;

    public AuthenticateController(IAuthService authService, ILogger<AuthenticateController> logger)
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
            var (status, message) = await _authService.Login(model);

            if (status == 0)
                return BadRequest(message);

            return Ok(message);
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
}
