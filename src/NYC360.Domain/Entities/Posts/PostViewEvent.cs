using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Entities.Posts;

public class PostViewEvent
{
    public int Id { get; set; }

    public int PostId { get; set; }
    public Post? Post { get; set; }

    public int? UserId { get; set; }
    public ApplicationUser? User { get; set; }
}