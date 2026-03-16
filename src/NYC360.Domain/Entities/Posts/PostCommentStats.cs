using System.ComponentModel.DataAnnotations;

namespace NYC360.Domain.Entities.Posts;

public class PostCommentStats
{
    [Key]
    public int CommentId { get; set; }
    public PostComment? Comment { get; set; }

    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public int Replies { get; set; }
}