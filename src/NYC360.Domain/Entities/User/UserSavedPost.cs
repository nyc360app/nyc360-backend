using NYC360.Domain.Entities.Posts;

namespace NYC360.Domain.Entities.User;

public class UserSavedPost
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public UserProfile? User { get; set; }

    public int PostId { get; set; }
    public Post? Post { get; set; }
    
    public DateTime SavedAt { get; set; }
}
