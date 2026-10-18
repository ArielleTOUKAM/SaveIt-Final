using SaveIt.Models;

namespace SaveIt.Services;

public interface INotificationService
{
    Task<IReadOnlyList<NotificationItem>> GetNotificationsAsync(bool unreadOnly = false);
    Task MarkAsReadAsync(string notificationId);
    Task<int> GetUnreadCountAsync();
}
