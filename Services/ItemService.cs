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
                Title = "Black Leather Wallet",
                Category = "Wallet",
                Location = "Library - 2nd Floor",
                Date = DateTime.Today.AddDays(-1),
                Type = ItemType.Lost,
                Status = ItemStatus.Active,
                Description = "Black genuine leather wallet containing student card and driver license. Small silver logo on front corner.",
                ReporterName = "Arielle S.",
                ReporterId = "user_1"
            },
            new Item
            {
                Id = "item_2",
                Title = "Black Leather Wallet",
                Category = "Wallet",
                Location = "Library Entrance / Hall",
                Date = DateTime.Today,
                Type = ItemType.Found,
                Status = ItemStatus.Active,
                Description = "Found a black leather wallet near the library stairs. Has silver badge.",
                ReporterName = "Jean D.",
                ReporterId = "user_2"
            },
            new Item
            {
                Id = "item_3",
                Title = "Silver Keychain & Car Key",
                Category = "Keys",
                Location = "Student Cafeteria",
                Date = DateTime.Today.AddDays(-2),
                Type = ItemType.Lost,
                Status = ItemStatus.Active,
                Description = "Set of 3 keys with a Peugeot car key and an Eiffel Tower metallic keychain.",
                ReporterName = "Arielle S.",
                ReporterId = "user_1"
            },
            new Item
            {
                Id = "item_4",
                Title = "Car Key with Keychain",
                Category = "Keys",
                Location = "Campus Cafeteria Terrace",
                Date = DateTime.Today.AddDays(-1),
                Type = ItemType.Found,
                Status = ItemStatus.Active,
                Description = "Found car key and 2 small silver keys near bench.",
                ReporterName = "Marc L.",
                ReporterId = "user_3"
            },
            new Item
            {
                Id = "item_5",
                Title = "Blue North Face Backpack",
                Category = "Bags",
                Location = "Lecture Hall B",
                Date = DateTime.Today.AddDays(-3),
                Type = ItemType.Found,
                Status = ItemStatus.Active,
                Description = "Navy blue backpack left after the 4 PM math lecture. Contains notebook and pencil case.",
                ReporterName = "Sophie M.",
                ReporterId = "user_4"
            },
            new Item
            {
                Id = "item_6",
                Title = "iPhone 13 Midnight Black",
                Category = "Electronics",
                Location = "Sports Complex Gym",
                Date = DateTime.Today.AddDays(-4),
                Type = ItemType.Lost,
                Status = ItemStatus.Active,
                Description = "Black iPhone with transparent silicone case, cracked screen protector on top right.",
                ReporterName = "David K.",
                ReporterId = "user_5"
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
            var dateMatch = dateDifferenceDays <= 3; // Within 3 days

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

            // Only return results having at least 2 matching criteria for relevance
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
                       .Where(w => w.Length > 3).ToHashSet();
        var words2 = p2.Split([' ', ',', '-', '/'], StringSplitOptions.RemoveEmptyEntries)
                       .Where(w => w.Length > 3);

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
