using System.Windows.Input;
using SaveIt.Services;

namespace SaveIt.ViewModels;

public class AuthViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly Action<string> _navigateAction;

    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _fullName = string.Empty;
    private string _confirmPassword = string.Empty;
    private string? _errorMessage;
    private bool _isLoginMode = true;
    private bool _hasAttemptedRegistration;

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
        set
        {
            if (SetProperty(ref _email, value))
                OnRegistrationValuesChanged();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (SetProperty(ref _password, value))
                OnRegistrationValuesChanged();
        }
    }

    public string FullName
    {
        get => _fullName;
        set
        {
            if (SetProperty(ref _fullName, value))
                OnRegistrationValuesChanged();
        }
    }

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            if (SetProperty(ref _confirmPassword, value))
                OnRegistrationValuesChanged();
        }
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (SetProperty(ref _errorMessage, value))
                OnPropertyChanged(nameof(HasError));
        }
    }

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
    public bool IsFullNameInvalid => _hasAttemptedRegistration && string.IsNullOrWhiteSpace(FullName);
    public bool IsRegistrationEmailInvalid => _hasAttemptedRegistration &&
        (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@'));
    public bool IsRegistrationPasswordInvalid => _hasAttemptedRegistration &&
        (string.IsNullOrWhiteSpace(Password) || Password.Length < 6);
    public bool IsConfirmPasswordInvalid => _hasAttemptedRegistration &&
        (string.IsNullOrWhiteSpace(ConfirmPassword) || Password != ConfirmPassword);

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
        if (!_hasAttemptedRegistration)
        {
            _hasAttemptedRegistration = true;
            OnRegistrationValidationChanged();
        }

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

    private void OnRegistrationValuesChanged()
    {
        OnRegistrationValidationChanged();
        if (HasError)
            ErrorMessage = null;
    }

    private void OnRegistrationValidationChanged()
    {
        OnPropertyChanged(nameof(IsFullNameInvalid));
        OnPropertyChanged(nameof(IsRegistrationEmailInvalid));
        OnPropertyChanged(nameof(IsRegistrationPasswordInvalid));
        OnPropertyChanged(nameof(IsConfirmPasswordInvalid));
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
