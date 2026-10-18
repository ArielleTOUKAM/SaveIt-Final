namespace SaveIt.Models;

public class ChatMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string ConversationId { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public bool IsFromCurrentUser { get; set; }

    public string TimeDisplay => Timestamp.ToString("HH:mm");
}

public class Conversation
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string OtherPartyName { get; set; } = string.Empty;
    public string RelatedItemId { get; set; } = string.Empty;
    public string RelatedItemTitle { get; set; } = string.Empty;
    public List<ChatMessage> Messages { get; set; } = [];
    public string LastMessageText => Messages.Count > 0 ? Messages[^1].Text : string.Empty;
    public DateTime LastActivity => Messages.Count > 0 ? Messages[^1].Timestamp : DateTime.Now;
}
