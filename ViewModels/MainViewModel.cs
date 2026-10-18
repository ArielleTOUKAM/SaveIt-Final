using System.Windows.Input;
using SaveIt.Models;
using SaveIt.Services;

namespace SaveIt.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly IItemService _itemService;
    private readonly IChatService _chatService;
    private readonly INotificationService _notificationService;
    private readonly IAuthService _authService;

    private string _currentScreen = "Splash";
    private string _selectedTab = "Home";
    private readonly Stack<string> _navigationHistory = new();

    public MainViewModel()
    {
        // Core Services
        _itemService = new ItemService();
        _chatService = new ChatService();
        _notificationService = new NotificationService();
        _authService = new AuthService();

        // Initialize Child ViewModels
        Auth = new AuthViewModel(_authService, (screen) => NavigateTo(screen));
        Home = new HomeViewModel(_itemService, (screen, param) => NavigateTo(screen, param));
        Report = new ReportViewModel(_itemService, (screen, param) => NavigateTo(screen, param));
        ItemDetails = new ItemDetailsViewModel(_itemService, (screen, param) => NavigateTo(screen, param), NavigateBack);
        Matches = new MatchesViewModel(_itemService, (screen, param) => NavigateTo(screen, param));
        Notifications = new NotificationsViewModel(_notificationService, _itemService, (screen, param) => NavigateTo(screen, param));
        Conversation = new ConversationViewModel(_chatService, NavigateBack);
        Profile = new ProfileViewModel(_authService, (screen, param) => NavigateTo(screen, param));

        // Tab Navigation Command
        SelectTabCommand = new Command<string>((tab) =>
        {
            if (string.IsNullOrWhiteSpace(tab)) return;
            SelectedTab = tab;
            switch (tab)
            {
                case "Home":
                    NavigateTo("Home");
                    break;
                case "Reports":
                    NavigateTo("Report", ItemType.Lost);
                    break;
                case "Matches":
                    NavigateTo("Matches");
                    break;
                case "Notifications":
                    NavigateTo("Notifications");
                    break;
                case "Profile":
                    NavigateTo("Profile");
                    break;
            }
        });

        // Initialize Splash Session routing
        _ = InitializeAppAsync();
    }

    public AuthViewModel Auth { get; }
    public HomeViewModel Home { get; }
    public ReportViewModel Report { get; }
    public ItemDetailsViewModel ItemDetails { get; }
    public MatchesViewModel Matches { get; }
    public NotificationsViewModel Notifications { get; }
    public ConversationViewModel Conversation { get; }
    public ProfileViewModel Profile { get; }

    public string CurrentScreen
    {
        get => _currentScreen;
        set
        {
            if (SetProperty(ref _currentScreen, value))
            {
                OnPropertyChanged(nameof(IsTabVisible));
                UpdateSelectedTabForScreen(value);
            }
        }
    }

    public string SelectedTab
    {
        get => _selectedTab;
        set => SetProperty(ref _selectedTab, value);
    }

    public bool IsTabVisible => CurrentScreen is "Home" or "Report" or "Matches" or "Notifications" or "Profile";

    public ICommand SelectTabCommand { get; }

    private async Task InitializeAppAsync()
    {
        // Eco-conception: brief, lightweight splash session check
        await Task.Delay(1100);
        if (_authService.IsSignedIn)
        {
            NavigateTo("Home");
        }
        else
        {
            NavigateTo("Login");
        }
    }

    public void NavigateTo(string screen, object? parameter = null)
    {
        // Enforce Back-to-origin rule: record origin if navigating to Details or Conversation
        if (screen is "Details" or "Conversation")
        {
            if (CurrentScreen is not ("Details" or "Conversation"))
            {
                _navigationHistory.Push(CurrentScreen);
            }
        }
        else if (screen is "Home" or "Matches" or "Notifications" or "Profile" or "Login")
        {
            _navigationHistory.Clear();
        }

        var previousScreen = CurrentScreen;
        CurrentScreen = screen;

        // Route dispatcher parameter handling
        switch (screen)
        {
            case "Home":
                _ = Home.LoadItemsAsync();
                break;

            case "Report":
                if (parameter is ItemType itemType)
                    Report.SetInitialType(itemType);
                break;

            case "Details":
                if (parameter is Item item)
                {
                    var origin = _navigationHistory.Count > 0 ? _navigationHistory.Peek() : "Home";
                    _ = ItemDetails.SetItemAsync(item, origin);
                }
                break;

            case "Matches":
                _ = Matches.LoadMatchesAsync(parameter as Item);
                break;

            case "Notifications":
                _ = Notifications.LoadNotificationsAsync();
                break;

            case "Conversation":
                var originConv = _navigationHistory.Count > 0 ? _navigationHistory.Peek() : "Home";
                if (parameter is ValueTuple<string, string, string> tuple)
                {
                    _ = Conversation.InitializeConversationAsync(tuple.Item1, tuple.Item2, tuple.Item3, originConv);
                }
                else
                {
                    _ = Conversation.InitializeConversationAsync("Jean D.", "item_1", "Black Leather Wallet", originConv);
                }
                break;

            case "Profile":
                Profile.RefreshProfile();
                break;
        }
    }

    public void NavigateBack()
    {
        if (_navigationHistory.Count > 0)
        {
            var destination = _navigationHistory.Pop();
            NavigateTo(destination);
        }
        else
        {
            NavigateTo("Home");
        }
    }

    private void UpdateSelectedTabForScreen(string screen)
    {
        SelectedTab = screen switch
        {
            "Home" => "Home",
            "Report" => "Reports",
            "Matches" => "Matches",
            "Notifications" => "Notifications",
            "Profile" => "Profile",
            _ => SelectedTab
        };
    }
}
