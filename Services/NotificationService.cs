using SaveIt.Models;

namespace SaveIt.Services;

public class NotificationService : INotificationService
{
    private readonly List<NotificationItem> _notifications = [];

    public NotificationService()
    {
        SeedNotifications();
    }

    private void SeedNotifications()
    {
        _notifications.AddRange(
        [
            new NotificationItem
            {
                Id = "notif_1",
                Title = "Potential Match Found!",
                Message = "A found 'Black Leather Wallet' matches 4/4 criteria of your lost report.",
                Timestamp = DateTime.Now.AddMinutes(-25),
                IsRead = false,
                Type = NotificationType.MatchAlert,
                TargetScreen = "Matches",
                RelatedItemId = "item_1"
            },
            new NotificationItem
            {
                Id = "notif_2",
                Title = "New message from Jean D.",
                Message = "Shall we meet at the campus main entrance gate at 5 PM?",
                Timestamp = DateTime.Now.AddMinutes(-10),
                IsRead = false,
                Type = NotificationType.MessageAlert,
                TargetScreen = "Conversation",
                RelatedConversationId = "conv_1"
            },
            new NotificationItem
            {
                Id = "notif_3",
                Title = "Report Status Update",
                Message = "Your declaration for 'Silver Keychain & Car Key' has 1 new prospective match.",
                Timestamp = DateTime.Now.AddHours(-3),
                IsRead = true,
                Type = NotificationType.ReportUpdate,
                TargetScreen = "Details",
                RelatedItemId = "item_3"
            }
        ]);
    }

    public Task<IReadOnlyList<NotificationItem>> GetNotificationsAsync(bool unreadOnly = false)
    {
        IEnumerable<NotificationItem> list = _notifications;
        if (unreadOnly)
            list = list.Where(n => !n.IsRead);

        return Task.FromResult<IReadOnlyList<NotificationItem>>(list.OrderByDescending(n => n.Timestamp).ToList());
    }

    public Task MarkAsReadAsync(string notificationId)
    {
        var notif = _notifications.FirstOrDefault(n => n.Id == notificationId);
        if (notif != null)
            notif.IsRead = true;

        return Task.CompletedTask;
    }

    public Task<int> GetUnreadCountAsync()
    {
        return Task.FromResult(_notifications.Count(n => !n.IsRead));
    }
}
