using GameCenter.Dtos.UserDtos;
using GameCenter.Models.User;

namespace GameCenter.Core.Services.AuthService
{

    public interface IAuthService
    {
        Task<(int, string)> Register(RegisterModel model, string role);
        Task<(int, string, RefreshToken?)> Login(LoginModel model);
        Task<(int, string, RefreshToken?)> RefreshToken(string email, string token);
        Task<List<UserDto>?> GetUsersList();
        Task<bool?> UpdateUserRole(string roleName, string userEmail);
        Task<bool?> DeleteUserByEmail(string userEmail);
        Task<UserDto?> GetUserDetails(string userEmail);
    }
}
