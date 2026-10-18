using SaveIt.Models;

namespace SaveIt.Services;

public interface IAuthService
{
    UserProfile CurrentUser { get; }
    bool IsSignedIn { get; }
    Task<bool> LoginAsync(string email, string password);
    Task<bool> RegisterAsync(string name, string email, string password);
    void Logout();
}
