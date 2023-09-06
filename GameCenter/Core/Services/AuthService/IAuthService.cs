using GameCenter.Models.User;

namespace GameCenter.Core.Services.AuthService
{

    public interface IAuthService
    {
        Task<(int, string)> Register(RegisterModel model, string role);
        Task<(int, string, RefreshToken?)> Login(LoginModel model);
        Task<(int, string, RefreshToken?)> RefreshToken(string userName, string token);
    }
}
