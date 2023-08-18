using GameCenter.Data;
using GameCenter.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameCenter.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly UserManager<GameCenterUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<GameCenterUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<(int, string)> Register(RegisterModel model, string role)
    {
        var userExists = await _userManager.FindByEmailAsync(model.Email);
        var usernameTaken = await _userManager.FindByNameAsync(model.Username);
        if (userExists != null)
        {
            return (0, "User with given email already exists");
        }
        if (usernameTaken != null)
        {
            return (0, "User with given Username already exists");
        }

        GameCenterUser user = new()
        {
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.Username,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return (0, "User creation failed! Please check user details and try again.");
        }

        if (!await _roleManager.RoleExistsAsync(role))
            await _roleManager.CreateAsync(new IdentityRole(role));
        if (await _roleManager.RoleExistsAsync(role))
            await _userManager.AddToRoleAsync(user, role);

        return (1, "User created successfully");

    }

    public async Task<(int, string)> Login(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return (0, "Invalid Email");
        if (!await _userManager.CheckPasswordAsync(user, model.Password))
            return (0, "Invalid Password");

        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        string token = GenerateToken(authClaims);

        return (1, token);
    }

    private string GenerateToken(IEnumerable<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Secret").Value!));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration.GetSection("Jwt:Issuer").Value,
            Audience = _configuration.GetSection("Jwt:Audience").Value,
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(authClaims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
