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
                Title = "New Match Found!",
                Message = "A found wallet may match your Campus report. Compare the details.",
                Timestamp = DateTime.Now.AddMinutes(-2),
                IsRead = false,
                Type = NotificationType.MatchAlert,
                TargetScreen = "Matches",
                RelatedItemId = "item_1"
            },
            new NotificationItem
            {
                Id = "notif_2",
                Title = "Jean D.",
                Message = "Hi! I found a black wallet near campus. Can you describe it?",
                Timestamp = DateTime.Now.AddMinutes(-15),
                IsRead = false,
                Type = NotificationType.MessageAlert,
                TargetScreen = "Conversation",
                RelatedConversationId = "conv_1"
            },
            new NotificationItem
            {
                Id = "notif_3",
                Title = "Report Updated",
                Message = "Your report \"Black wallet — Campus\" is active.",
                Timestamp = DateTime.Now.AddHours(-1),
                IsRead = true,
                Type = NotificationType.ReportUpdate,
                TargetScreen = "Details",
                RelatedItemId = "item_1"
            },
            new NotificationItem
            {
                Id = "notif_4",
                Title = "Possible Match",
                Message = "A found item shares 2 of 4 criteria with your keys report.",
                Timestamp = DateTime.Now.AddDays(-1),
                IsRead = true,
                Type = NotificationType.MatchAlert,
                TargetScreen = "Matches",
                RelatedItemId = "item_3"
            },
            new NotificationItem
            {
                Id = "notif_5",
                Title = "Thomas K.",
                Message = "I left it at the lost & found desk of Galeries Bonamoussadi, you can pick it up anytime.",
                Timestamp = DateTime.Now.AddDays(-1),
                IsRead = true,
                Type = NotificationType.MessageAlert,
                TargetScreen = "Conversation",
                RelatedConversationId = "conv_2"
            },
            new NotificationItem
            {
                Id = "notif_6",
                Title = "Reminder",
                Message = "Your report \"Silver Keys, Opera\" hasn't been updated in 7 days. Review it to keep it active.",
                Timestamp = DateTime.Now.AddDays(-1),
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
