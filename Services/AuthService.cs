using SaveIt.Models;

namespace SaveIt.Services;

public class AuthService : IAuthService
{
    private UserProfile _currentUser;
    private bool _isSignedIn = true; // Initially true for demo session, can be logged out

    public AuthService()
    {
        _currentUser = new UserProfile
        {
            Id = "user_1",
            FullName = "Arielle Seumegne",
            Email = "arielle@gmail.com",
            Role = "Student / Campus Community",
            LostReportsCount = 2,
            FoundReportsCount = 3,
            ActiveMatchesCount = 2,
            ReunitedCount = 1
        };
    }

    public UserProfile CurrentUser => _currentUser;
    public bool IsSignedIn => _isSignedIn;

    public Task<bool> LoginAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            return Task.FromResult(false);

        _isSignedIn = true;
        _currentUser.Email = email.Trim();
        return Task.FromResult(true);
    }

    public Task<bool> RegisterAsync(string name, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            return Task.FromResult(false);

        _isSignedIn = true;
        _currentUser.FullName = name.Trim();
        _currentUser.Email = email.Trim();
        return Task.FromResult(true);
    }

    public void Logout()
    {
        _isSignedIn = false;
    }
}
