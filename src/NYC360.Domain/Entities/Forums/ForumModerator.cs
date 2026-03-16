using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Entities.Forums;

public class ForumModerator
{
    public int Id { get; set; }

    public int ForumId { get; set; }
    public Forum Forum { get; set; } = null!;

    public int ModeratorId { get; set; }
    public UserProfile Moderator { get; set; } = null!;

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
