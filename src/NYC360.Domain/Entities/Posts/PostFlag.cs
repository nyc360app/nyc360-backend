using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums;

namespace NYC360.Domain.Entities.Posts;

public class PostFlag
{
    public int Id { get; set; }

    // Relationship to the reported content
    public int PostId { get; set; }
    public Post Post { get; set; } = null!;

    // Relationship to the reporting user
    public int UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    // Details of the flag
    public FlagReasonType Reason { get; set; }
    public string? Details { get; set; }

    // Moderation status
    public FlagStatus Status { get; set; } = FlagStatus.Pending;
    public string? ReviewerNote{ get; set; }
    public int? ReviewerId { get; set; }
    public ApplicationUser? Reviewer { get; set; }
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReviewedAt { get; set; }
}