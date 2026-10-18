namespace SaveIt.Models;

public class UserProfile
{
    public string Id { get; set; } = "user_1";
    public string FullName { get; set; } = "Arielle Seumegne";
    public string Email { get; set; } = "arielle@gmail.com";
    public string Role { get; set; } = "Student / Campus Community";
    public DateTime MemberSince { get; set; } = new(2026, 1, 15);

    public int LostReportsCount { get; set; } = 2;
    public int FoundReportsCount { get; set; } = 3;
    public int ActiveMatchesCount { get; set; } = 4;
    public int ReunitedCount { get; set; } = 1;

    public string Initials
    {
        get
        {
            if (string.IsNullOrWhiteSpace(FullName)) return "U";
            var parts = FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length switch
            {
                >= 2 => $"{parts[0][0]}{parts[1][0]}".ToUpper(),
                1 => parts[0][0].ToString().ToUpper(),
                _ => "U"
            };
        }
    }
}
