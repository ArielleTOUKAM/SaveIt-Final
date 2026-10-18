using SaveIt.Models;

namespace SaveIt.Services;

public class ItemService : IItemService
{
    private readonly List<Item> _items = [];

    public ItemService()
    {
        SeedData();
    }

    private void SeedData()
    {
        _items.AddRange(
        [
            new Item
            {
                Id = "item_1",
                Title = "Black Wallet",
                Category = "Accessories",
                Location = "Campus",
                Date = new DateTime(2026, 10, 4),
                Type = ItemType.Lost,
                Status = ItemStatus.Active,
                Description = "Black genuine leather wallet with student card and driver license. Small silver logo on corner.",
                ReporterName = "Arielle S.",
                ReporterId = "user_1"
            },
            new Item
            {
                Id = "item_2",
                Title = "Blue Jacket",
                Category = "Clothing",
                Location = "Ndokoti",
                Date = new DateTime(2026, 10, 5),
                Type = ItemType.Found,
                Status = ItemStatus.Active,
                Description = "Denim blue jacket left on a bench near Ndokoti market intersection.",
                ReporterName = "Jean D.",
                ReporterId = "user_2"
            },
            new Item
            {
                Id = "item_3",
                Title = "Headphones",
                Category = "Electronics",
                Location = "Village Shell",
                Date = new DateTime(2026, 10, 5),
                Type = ItemType.Lost,
                Status = ItemStatus.Active,
                Description = "Wireless black over-ear headphones in a dark protective zip pouch.",
                ReporterName = "Marc L.",
                ReporterId = "user_3"
            },
            new Item
            {
                Id = "item_4",
                Title = "Black wallet",
                Category = "Accessories",
                Location = "Campus",
                Date = new DateTime(2026, 10, 5),
                Type = ItemType.Found,
                Status = ItemStatus.Active,
                Description = "Found black wallet near campus entrance stairs. Contains cardholder.",
                ReporterName = "Jean D.",
                ReporterId = "user_2"
            }
        ]);
    }

    public Task<IReadOnlyList<Item>> GetItemsAsync(string filter = "All", string searchQuery = "")
    {
        IEnumerable<Item> query = _items;

        if (filter.Equals("Lost", StringComparison.OrdinalIgnoreCase))
            query = query.Where(i => i.Type == ItemType.Lost);
        else if (filter.Equals("Found", StringComparison.OrdinalIgnoreCase))
            query = query.Where(i => i.Type == ItemType.Found);
        else if (filter is "Electronics" or "Clothing" or "Documents" or "Accessories")
            query = query.Where(i => i.Category.Equals(filter, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            var q = searchQuery.Trim();
            query = query.Where(i =>
                i.Title.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                i.Category.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                i.Location.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                i.Description.Contains(q, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult<IReadOnlyList<Item>>(query.OrderByDescending(i => i.Date).ToList());
    }

    public Task<Item?> GetItemByIdAsync(string id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id || i.Title.Equals(id, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(item);
    }

    public Task<Item> AddItemAsync(Item item)
    {
        if (string.IsNullOrWhiteSpace(item.Id))
            item.Id = Guid.NewGuid().ToString("N");

        _items.Insert(0, item);
        return Task.FromResult(item);
    }

    /// <summary>
    /// Deterministic matching using the 4 explicit criteria: Category, Place, Date, Description (No AI).
    /// </summary>
    public Task<IReadOnlyList<MatchResult>> FindMatchesAsync(Item sourceItem)
    {
        var targetType = sourceItem.Type == ItemType.Lost ? ItemType.Found : ItemType.Lost;
        var candidates = _items.Where(i => i.Type == targetType && i.Id != sourceItem.Id).ToList();

        var results = new List<MatchResult>();

        foreach (var candidate in candidates)
        {
            var categoryMatch = string.Equals(sourceItem.Category, candidate.Category, StringComparison.OrdinalIgnoreCase);
            var placeMatch = IsSimilarPlace(sourceItem.Location, candidate.Location);
            var dateDifferenceDays = Math.Abs((sourceItem.Date.Date - candidate.Date.Date).TotalDays);
            var dateMatch = dateDifferenceDays <= 2;
            var descriptionMatch = HasCommonKeywords(
                $"{sourceItem.Title} {sourceItem.Description}",
                $"{candidate.Title} {candidate.Description}");

            var match = new MatchResult
            {
                SourceItem = sourceItem,
                CandidateItem = candidate,
                CategoryMatch = categoryMatch,
                PlaceMatch = placeMatch,
                DateMatch = dateMatch,
                DescriptionMatch = descriptionMatch
            };

            if (match.MatchingCriteriaCount >= 2)
            {
                results.Add(match);
            }
        }

        return Task.FromResult<IReadOnlyList<MatchResult>>(
            results.OrderByDescending(r => r.MatchingCriteriaCount).ToList());
    }

    public async Task<IReadOnlyList<MatchResult>> GetAllMatchesAsync()
    {
        var allMatches = new List<MatchResult>();
        var userLostItems = _items.Where(i => i.Type == ItemType.Lost && (i.ReporterId == "user_1" || i.ReporterName == "Arielle S.")).ToList();

        foreach (var lostItem in userLostItems)
        {
            var matches = await FindMatchesAsync(lostItem);
            allMatches.AddRange(matches);
        }

        return allMatches.DistinctBy(m => $"{m.SourceItem.Id}_{m.CandidateItem.Id}").ToList();
    }

    private static bool IsSimilarPlace(string place1, string place2)
    {
        if (string.IsNullOrWhiteSpace(place1) || string.IsNullOrWhiteSpace(place2)) return false;
        var p1 = place1.ToLowerInvariant();
        var p2 = place2.ToLowerInvariant();

        if (p1.Contains(p2) || p2.Contains(p1)) return true;

        var words1 = p1.Split([' ', ',', '-', '/'], StringSplitOptions.RemoveEmptyEntries)
                       .Where(w => w.Length > 2).ToHashSet();
        var words2 = p2.Split([' ', ',', '-', '/'], StringSplitOptions.RemoveEmptyEntries)
                       .Where(w => w.Length > 2);

        return words2.Any(words1.Contains);
    }

    private static bool HasCommonKeywords(string text1, string text2)
    {
        var stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "the", "and", "with", "found", "lost", "near", "small", "very", "this", "that",
            "le", "la", "les", "un", "une", "des", "dans", "sur", "avec", "pour"
        };

        var tokens1 = text1.ToLowerInvariant()
            .Split([' ', ',', '.', '!', '?', '-', '(', ')', '/'], StringSplitOptions.RemoveEmptyEntries)
            .Where(w => w.Length >= 3 && !stopWords.Contains(w))
            .ToHashSet();

        var tokens2 = text2.ToLowerInvariant()
            .Split([' ', ',', '.', '!', '?', '-', '(', ')', '/'], StringSplitOptions.RemoveEmptyEntries)
            .Where(w => w.Length >= 3 && !stopWords.Contains(w));

        return tokens2.Any(tokens1.Contains);
    }
}
