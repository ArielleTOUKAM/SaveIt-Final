using System.Windows.Input;
using SaveIt.Services;

namespace SaveIt.ViewModels;

public class AuthViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly Action<string> _navigateAction;

    private string _email = "arielle@gmail.com";
    private string _password = "Password123";
    private string _fullName = "Arielle Seumegne";
    private string _confirmPassword = "Password123";
    private string? _errorMessage;
    private bool _isLoginMode = true;

    public AuthViewModel(IAuthService authService, Action<string> navigateAction)
    {
        _authService = authService;
        _navigateAction = navigateAction;

        LoginCommand = new Command(async () => await ExecuteLoginAsync());
        RegisterCommand = new Command(async () => await ExecuteRegisterAsync());
        ToggleModeCommand = new Command(ToggleMode);
        ForgotPasswordCommand = new Command(ExecuteForgotPassword);
        GoogleSignInCommand = new Command(ExecuteGoogleSignIn);
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string FullName
    {
        get => _fullName;
        set => SetProperty(ref _fullName, value);
    }

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value);
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public bool IsLoginMode
    {
        get => _isLoginMode;
        set => SetProperty(ref _isLoginMode, value);
    }

    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }
    public ICommand ToggleModeCommand { get; }
    public ICommand ForgotPasswordCommand { get; }
    public ICommand GoogleSignInCommand { get; }

    private void ToggleMode()
    {
        IsLoginMode = !IsLoginMode;
        ErrorMessage = null;
        _navigateAction(IsLoginMode ? "Login" : "Registration");
    }

    private async Task ExecuteLoginAsync()
    {
        ErrorMessage = null;
        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@'))
        {
            ErrorMessage = "Please enter a valid email address.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
        {
            ErrorMessage = "Password must be at least 6 characters.";
            return;
        }

        IsBusy = true;
        var success = await _authService.LoginAsync(Email, Password);
        IsBusy = false;

        if (success)
        {
            _navigateAction("Home");
        }
        else
        {
            ErrorMessage = "Invalid credentials. Please verify.";
        }
    }

    private async Task ExecuteRegisterAsync()
    {
        ErrorMessage = null;
        if (string.IsNullOrWhiteSpace(FullName))
        {
            ErrorMessage = "Please enter your full name.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@'))
        {
            ErrorMessage = "Please enter a valid email address.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
        {
            ErrorMessage = "Password must be at least 6 characters.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Passwords do not match.";
            return;
        }

        IsBusy = true;
        var success = await _authService.RegisterAsync(FullName, Email, Password);
        IsBusy = false;

        if (success)
        {
            _navigateAction("Home");
        }
        else
        {
            ErrorMessage = "Registration failed. Try again.";
        }
    }

    private void ExecuteForgotPassword()
    {
        ErrorMessage = "A password recovery email has been sent.";
    }

    private void ExecuteGoogleSignIn()
    {
        _authService.LoginAsync("arielle@gmail.com", "google_oauth");
        _navigateAction("Home");
    }
}
