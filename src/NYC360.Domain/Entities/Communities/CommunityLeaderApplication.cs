using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums.Communities;

namespace NYC360.Domain.Entities.Communities;

public class CommunityLeaderApplication
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserProfile? User { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string CommunityName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string? ProfileLink { get; set; }
    public string Motivation { get; set; } = string.Empty;
    public string Experience { get; set; } = string.Empty;
    public bool LedBefore { get; set; }
    public string WeeklyAvailability { get; set; } = string.Empty;
    public bool AgreedToGuidelines { get; set; }
    public string VerificationFileUrl { get; set; } = string.Empty;
    public CommunityLeaderApplicationStatus Status { get; set; } = CommunityLeaderApplicationStatus.Pending;
    public string? AdminNotes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReviewedAt { get; set; }
}
