using SaveIt.Models;

namespace SaveIt.Services;

public interface IChatService
{
    Task<Conversation> GetOrCreateConversationAsync(string otherPartyName, string relatedItemId, string relatedItemTitle);
    Task<ChatMessage> SendMessageAsync(string conversationId, string text);
    Task<IReadOnlyList<Conversation>> GetAllConversationsAsync();
}
