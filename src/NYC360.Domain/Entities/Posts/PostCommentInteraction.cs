using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums;

namespace NYC360.Domain.Entities.Posts;

public class PostCommentInteraction
{
    public int Id { get; set; }

    public int CommentId { get; set; }
    public PostComment? Comment { get; set; }

    public int UserId { get; set; }
    public ApplicationUser? User { get; set; }
    
    public CommentInteractionType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}