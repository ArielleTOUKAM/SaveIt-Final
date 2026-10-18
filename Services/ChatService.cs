using SaveIt.Models;

namespace SaveIt.Services;

public class ChatService : IChatService
{
    private readonly List<Conversation> _conversations = [];

    public ChatService()
    {
        SeedConversations();
    }

    private void SeedConversations()
    {
        _conversations.Add(new Conversation
        {
            Id = "conv_1",
            OtherPartyName = "Jean D.",
            RelatedItemId = "item_1",
            RelatedItemTitle = "Black leather wallet",
            Messages =
            [
                new ChatMessage
                {
                    Id = "msg_1",
                    ConversationId = "conv_1",
                    SenderName = "Jean D.",
                    Text = "Hello! I found a black wallet near campus. Is yours black leather?",
                    Timestamp = DateTime.Today.AddHours(10).AddMinutes(12),
                    IsFromCurrentUser = false
                },
                new ChatMessage
                {
                    Id = "msg_2",
                    ConversationId = "conv_1",
                    SenderName = "Arielle T.",
                    Text = "Yes, with a small silver logo. Thank you!",
                    Timestamp = DateTime.Today.AddHours(10).AddMinutes(15),
                    IsFromCurrentUser = true
                },
                new ChatMessage
                {
                    Id = "msg_3",
                    ConversationId = "conv_1",
                    SenderName = "Jean D.",
                    Text = "That matches. Shall we meet at the campus gate at 5 PM?",
                    Timestamp = DateTime.Today.AddHours(10).AddMinutes(18),
                    IsFromCurrentUser = false
                },
                new ChatMessage
                {
                    Id = "msg_4",
                    ConversationId = "conv_1",
                    SenderName = "Arielle T.",
                    Text = "Perfect, 5 PM works for me!",
                    Timestamp = DateTime.Today.AddHours(10).AddMinutes(19),
                    IsFromCurrentUser = true
                },
                new ChatMessage
                {
                    Id = "msg_5",
                    ConversationId = "conv_1",
                    SenderName = "Arielle T.",
                    Text = "See you then!",
                    Timestamp = DateTime.Today.AddHours(10).AddMinutes(20),
                    IsFromCurrentUser = true
                }
            ]
        });
    }

    public Task<Conversation> GetOrCreateConversationAsync(string otherPartyName, string relatedItemId, string relatedItemTitle)
    {
        var conv = _conversations.FirstOrDefault(c =>
            string.Equals(c.OtherPartyName, otherPartyName, StringComparison.OrdinalIgnoreCase) ||
            c.RelatedItemId == relatedItemId);

        if (conv == null)
        {
            conv = new Conversation
            {
                Id = Guid.NewGuid().ToString("N"),
                OtherPartyName = otherPartyName,
                RelatedItemId = relatedItemId,
                RelatedItemTitle = relatedItemTitle,
                Messages =
                [
                    new ChatMessage
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        SenderName = otherPartyName,
                        Text = $"Hello, I saw your report regarding '{relatedItemTitle}'. Let's discuss safely here.",
                        Timestamp = DateTime.Now,
                        IsFromCurrentUser = false
                    }
                ]
            };
            _conversations.Insert(0, conv);
        }

        return Task.FromResult(conv);
    }

    public Task<ChatMessage> SendMessageAsync(string conversationId, string text)
    {
        var conv = _conversations.FirstOrDefault(c => c.Id == conversationId);
        if (conv == null)
        {
            conv = new Conversation { Id = conversationId, OtherPartyName = "Finder / Owner" };
            _conversations.Add(conv);
        }

        var message = new ChatMessage
        {
            Id = Guid.NewGuid().ToString("N"),
            ConversationId = conversationId,
            SenderName = "Arielle T.",
            Text = text.Trim(),
            Timestamp = DateTime.Now,
            IsFromCurrentUser = true
        };

        conv.Messages.Add(message);
        return Task.FromResult(message);
    }

    public Task<IReadOnlyList<Conversation>> GetAllConversationsAsync()
    {
        return Task.FromResult<IReadOnlyList<Conversation>>(_conversations.OrderByDescending(c => c.LastActivity).ToList());
    }
}
