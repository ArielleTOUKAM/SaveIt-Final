using SaveIt.Models;

namespace SaveIt.Services;

public interface IItemService
{
    Task<IReadOnlyList<Item>> GetItemsAsync(string filter = "All", string searchQuery = "");
    Task<Item?> GetItemByIdAsync(string id);
    Task<Item> AddItemAsync(Item item);
    Task<IReadOnlyList<MatchResult>> FindMatchesAsync(Item sourceItem);
    Task<IReadOnlyList<MatchResult>> GetAllMatchesAsync();
}
