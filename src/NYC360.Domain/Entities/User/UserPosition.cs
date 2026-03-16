namespace NYC360.Domain.Entities.User;

public class UserPosition
{
    public int Id { get; set; }
    public int UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }

    public string Title { get; set; } = string.Empty; // e.g., Senior Developer
    public string Company { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrent { get; set; }
}