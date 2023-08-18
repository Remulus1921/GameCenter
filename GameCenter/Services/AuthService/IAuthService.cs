using GameCenter.Models.User;

namespace GameCenter.Services.AuthService;

public interface IAuthService
{
    Task<(int, string)> Register(RegisterModel model, string role);
    Task<(int, string)> Login(LoginModel model);
}
