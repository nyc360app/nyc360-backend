using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Posts;

namespace NYC360.Domain.Entities.Posts;

public class PostInteraction
{
    public int Id { get; set; }

    public int PostId { get; set; }
    public Post? Post { get; set; }

    public int UserId { get; set; }
    public UserProfile? User { get; set; }
    
    public InteractionType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}