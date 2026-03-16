using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Entities.Posts;

public class PostComment
{
    public int Id { get; set; }

    public int PostId { get; set; }
    public Post? Post { get; set; }

    public int UserId { get; set; }
    public UserProfile? User { get; set; }
    
    public int? ParentCommentId { get; set; }
    public PostComment? ParentComment { get; set; }
    
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    public PostCommentStats? Stats { get; set; } = new PostCommentStats();
    public ICollection<PostComment> Replies { get; set; } = new List<PostComment>();
    public ICollection<PostCommentInteraction> Interactions { get; set; } = new List<PostCommentInteraction>();
    
}