using GameCenter.Data;
using GameCenter.Dtos.UserDtos;
using GameCenter.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
                return (0, "Email zajęty");
            }
            if (usernameTaken != null)
            {
                return (0, "Nazwa użytkownika zajęta");
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
                return (0, "Nie udało się stworzyć konta! Proszę sprawdź jeszcze raz dane i spróbuj ponownie.");
            }

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));
            if (await _roleManager.RoleExistsAsync(role))
                await _userManager.AddToRoleAsync(user, role);

            return (1, "Użytkownik stworzony pomyślnie");

        }

        public async Task<(int, string, RefreshToken?)> Login(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return (0, "Nie prawidłowy login lub hasło", null);
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return (0, "Nie prawidłowy login lub hasło", null);

            string token = await GenerateToken(user);

            var refreshToken = GenerateRefreshToken();
            refreshToken.UserId = user.Id;

            _context.Add(refreshToken);
            _context.SaveChanges();

            return (1, token, refreshToken);
        }

        public async Task<(int, string, RefreshToken?)> RefreshToken(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return (0, "Zaloguj się jeszcze raz", null);

            var refreshToken = _context.RefreshTokens.FirstOrDefault(r => r.Token == token && r.UserId == user.Id);
            if (refreshToken == null) return (0, "Zaloguj się jeszcze raz", null);
            else if (refreshToken.IsValid == false)
            {
                var childToken = _context.RefreshTokens.FirstOrDefault(r => r.ParentId == refreshToken.Id);
                while (childToken != null)
                {
                    childToken.IsValid = false;
                    childToken = _context.RefreshTokens.FirstOrDefault(r => r.ParentId == childToken.Id);
                }
                return (0, "Zaloguj się jeszcze raz", null);
            }

            if (refreshToken.Expires < DateTime.Now) return (0, "Zaloguj się jeszcze raz", null);

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
                Expires = DateTime.Now.AddMinutes(10),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(authClaims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<List<UserDto>?> GetUsersList()
        {
            var users = await _context.Users.Select(u => new
            {
                User = u,
                Roles = _context.UserRoles
                    .Where(ur => ur.UserId == u.Id)
                    .Join(_context.Roles, ur => ur.RoleId, role => role.Id, (ur, role) => role.Name)
                    .ToList()
            })
            .ToListAsync();

            if (users.Count == 0)
            {
                return null;
            }

            var usersList = new List<UserDto>();

            foreach (var user in users)
            {
                var userDto = new UserDto
                {
                    UserName = user.User.UserName,
                    UserEmail = user.User.Email,
                    UserRole = user.Roles.First()
                };

                usersList.Add(userDto);
            }

            return usersList;
        }

        public async Task<bool?> UpdateUserRole(string roleName, string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return false;
            }

            var currRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in currRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            if (await _roleManager.RoleExistsAsync(roleName))
            {
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }

            return false;
        }

        public async Task<bool?> DeleteUserByEmail(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
                return null;

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return true;
            else return false;
        }

        public async Task<UserDto?> GetUserDetails(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
                return null;

            var role = _context.UserRoles.Where(ur => ur.UserId == user.Id).Join(_context.Roles, ur => ur.RoleId, role => role.Id, (ur, role) => role.Name).ToList();

            var userDto = new UserDto
            {
                UserEmail = user.Email,
                UserName = user.UserName,
                UserRole = role.First()
            };

            return userDto;
        }
    }
}
