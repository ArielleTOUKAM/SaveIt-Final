using System.Windows.Input;
using SaveIt.Models;
using SaveIt.Services;

namespace SaveIt.ViewModels;

public class ProfileViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly Action<string, object?> _navigateAction;

    private UserProfile _userProfile;
    private string _activeSection = "Overview"; // "Overview", "Preferences", "Privacy", "Help"

    public ProfileViewModel(IAuthService authService, Action<string, object?> navigateAction)
    {
        _authService = authService;
        _navigateAction = navigateAction;
        _userProfile = authService.CurrentUser;

        ViewLostItemsCommand = new Command(() => _navigateAction("Report", ItemType.Lost));
        ViewFoundItemsCommand = new Command(() => _navigateAction("Report", ItemType.Found));
        ViewMatchesCommand = new Command(() => _navigateAction("Matches", null));
        ViewNotificationsCommand = new Command(() => _navigateAction("Notifications", null));
        SelectSectionCommand = new Command<string>((section) =>
        {
            if (section != null)
                ActiveSection = section;
        });
        LogoutCommand = new Command(() =>
        {
            _authService.Logout();
            _navigateAction("Login", null);
        });
    }

    public UserProfile UserProfile
    {
        get => _userProfile;
        set => SetProperty(ref _userProfile, value);
    }

    public string ActiveSection
    {
        get => _activeSection;
        set => SetProperty(ref _activeSection, value);
    }

    public ICommand ViewLostItemsCommand { get; }
    public ICommand ViewFoundItemsCommand { get; }
    public ICommand ViewMatchesCommand { get; }
    public ICommand ViewNotificationsCommand { get; }
    public ICommand SelectSectionCommand { get; }
    public ICommand LogoutCommand { get; }

    public void RefreshProfile()
    {
        UserProfile = _authService.CurrentUser;
    }
}
