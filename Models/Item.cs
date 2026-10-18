namespace SaveIt.Models;

public enum ItemType
{
    Lost,
    Found
}

public enum ItemStatus
{
    Active,
    Resolved
}

public class Item
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Today;
    public ItemType Type { get; set; } = ItemType.Lost;
    public ItemStatus Status { get; set; } = ItemStatus.Active;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string ReporterName { get; set; } = string.Empty;
    public string ReporterId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string TypeDisplay => Type == ItemType.Lost ? "Lost" : "Found";
    public string DateDisplay => Date.ToString("dd MMM yyyy");
    public string StatusDisplay => Status == ItemStatus.Active ? "Open" : "Resolved";
    
    // Eco-conception: Color tokens strictly based on the design system
    public string StatusColor => Type == ItemType.Lost ? "#E5484D" : "#15965B";
    public string StatusBgColor => Type == ItemType.Lost ? "#FEECEE" : "#EAF7ED";
}
