using System.Collections.ObjectModel;
using System.Windows.Input;
using SaveIt.Models;
using SaveIt.Services;

namespace SaveIt.ViewModels;

public class NotificationsViewModel : BaseViewModel
{
    private readonly INotificationService _notificationService;
    private readonly IItemService _itemService;
    private readonly Action<string, object?> _navigateAction;

    private string _filter = "All"; // "All", "Unread"
    private bool _isEmpty;
    private int _unreadCount;

    public NotificationsViewModel(
        INotificationService notificationService,
        IItemService itemService,
        Action<string, object?> navigateAction)
    {
        _notificationService = notificationService;
        _itemService = itemService;
        _navigateAction = navigateAction;

        Notifications = [];

        SelectFilterCommand = new Command<string>(async (f) =>
        {
            if (f == null) return;
            Filter = f;
            await LoadNotificationsAsync();
        });

        SelectNotificationCommand = new Command<NotificationItem>(async (item) =>
        {
            if (item == null) return;
            await _notificationService.MarkAsReadAsync(item.Id);
            item.IsRead = true;
            await RefreshUnreadCountAsync();

            switch (item.Type)
            {
                case NotificationType.MatchAlert:
                    if (!string.IsNullOrEmpty(item.RelatedItemId))
                    {
                        var related = await _itemService.GetItemByIdAsync(item.RelatedItemId);
                        _navigateAction("Matches", related);
                    }
                    else
                    {
                        _navigateAction("Matches", null);
                    }
                    break;

                case NotificationType.MessageAlert:
                    _navigateAction("Conversation", ("Jean D.", item.RelatedItemId ?? "item_1", "Black Leather Wallet"));
                    break;

                case NotificationType.ReportUpdate:
                    if (!string.IsNullOrEmpty(item.RelatedItemId))
                    {
                        var related = await _itemService.GetItemByIdAsync(item.RelatedItemId);
                        if (related != null)
                            _navigateAction("Details", related);
                        else
                            _navigateAction("Home", null);
                    }
                    break;
            }
        });
    }

    public ObservableCollection<NotificationItem> Notifications { get; }

    public string Filter
    {
        get => _filter;
        set => SetProperty(ref _filter, value);
    }

    public bool IsEmpty
    {
        get => _isEmpty;
        set => SetProperty(ref _isEmpty, value);
    }

    public int UnreadCount
    {
        get => _unreadCount;
        set => SetProperty(ref _unreadCount, value);
    }

    public ICommand SelectFilterCommand { get; }
    public ICommand SelectNotificationCommand { get; }

    public async Task LoadNotificationsAsync()
    {
        IsBusy = true;
        Notifications.Clear();
        var list = await _notificationService.GetNotificationsAsync(Filter == "Unread");
        foreach (var n in list)
        {
            Notifications.Add(n);
        }

        IsEmpty = Notifications.Count == 0;
        await RefreshUnreadCountAsync();
        IsBusy = false;
    }

    private async Task RefreshUnreadCountAsync()
    {
        UnreadCount = await _notificationService.GetUnreadCountAsync();
    }
}
