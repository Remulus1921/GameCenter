using GameCenter.Data;
using GameCenter.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GameCenter.Core.Services.AuthService
{

    public class AuthService : IAuthService
    {
        private readonly UserManager<GameCenterUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthService(
            UserManager<GameCenterUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
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

        public async Task<(int, string, RefreshToken?)> Login(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return (0, "Email or Password was invalid", null);
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return (0, "Email or Password was invalid", null);

            string token = await GenerateToken(user);

            var refreshToken = GenerateRefreshToken();
            refreshToken.UserId = user.Id;

            _context.Add(refreshToken);
            _context.SaveChanges();

            return (1, token, refreshToken);
        }

        public async Task<(int, string, RefreshToken?)> RefreshToken(string userName, string token)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return (0, "Invalid Username", null);

            var refreshToken = _context.RefreshTokens.FirstOrDefault(r => r.Token == token && r.UserId == user.Id);
            if (refreshToken == null) return (0, "There is no such token", null);
            else if (refreshToken.IsValid == false)
            {
                var childToken = _context.RefreshTokens.FirstOrDefault(r => r.ParentId == refreshToken.Id);
                while (childToken != null)
                {
                    childToken.IsValid = false;
                    childToken = _context.RefreshTokens.FirstOrDefault(r => r.ParentId == childToken.Id);
                }
                return (0, "Refresh token got compromised relog", null);
            }

            if (refreshToken.Expires < DateTime.Now) return (0, "Token expired", null);

            string newToken = await GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();

            refreshToken.IsValid = false;

            newRefreshToken.UserId = user.Id;
            newRefreshToken.ParentId = refreshToken.Id;

            _context.Add(newRefreshToken);
            _context.SaveChanges();

            return (1, newToken, newRefreshToken);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7)
            };

            return refreshToken;
        }

        private async Task<string> GenerateToken(GameCenterUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Secret").Value!));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration.GetSection("Jwt:Issuer").Value,
                Audience = _configuration.GetSection("Jwt:Audience").Value,
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(authClaims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


    }
}
