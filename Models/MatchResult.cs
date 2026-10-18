namespace SaveIt.Models;

public class MatchResult
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public Item SourceItem { get; set; } = new();
    public Item CandidateItem { get; set; } = new();

    public bool CategoryMatch { get; set; }
    public bool PlaceMatch { get; set; }
    public bool DateMatch { get; set; }
    public bool DescriptionMatch { get; set; }

    public int MatchingCriteriaCount =>
        (CategoryMatch ? 1 : 0) +
        (PlaceMatch ? 1 : 0) +
        (DateMatch ? 1 : 0) +
        (DescriptionMatch ? 1 : 0);

    public int ScorePercentage => (int)Math.Round((MatchingCriteriaCount / 4.0) * 100);

    public string ScoreDisplay => $"{MatchingCriteriaCount}/4 criteria match ({ScorePercentage}%)";

    public string CriteriaSummary
    {
        get
        {
            var matched = new List<string>();
            if (CategoryMatch) matched.Add("Category");
            if (PlaceMatch) matched.Add("Place");
            if (DateMatch) matched.Add("Date");
            if (DescriptionMatch) matched.Add("Keywords");
            return matched.Count > 0 ? string.Join(" • ", matched) : "No criteria matched";
        }
    }
}
