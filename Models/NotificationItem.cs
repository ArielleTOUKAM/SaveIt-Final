namespace SaveIt.Models;

public enum NotificationType
{
    MatchAlert,
    MessageAlert,
    ReportUpdate
}

public class NotificationItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public bool IsRead { get; set; }
    public NotificationType Type { get; set; } = NotificationType.MatchAlert;
    
    // Navigation routing targets
    public string TargetScreen { get; set; } = "Matches"; // "Matches", "Conversation", "Details"
    public string? RelatedItemId { get; set; }
    public string? RelatedConversationId { get; set; }

    public string TimeAgoDisplay
    {
        get
        {
            var span = DateTime.Now - Timestamp;
            if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes}m ago";
            if (span.TotalHours < 24) return $"{(int)span.TotalHours}h ago";
            return $"{(int)span.TotalDays}d ago";
        }
    }

    public string Icon => Type switch
    {
        NotificationType.MatchAlert => "♡",
        NotificationType.MessageAlert => "✉",
        NotificationType.ReportUpdate => "ℹ",
        _ => "♧"
    };
}
