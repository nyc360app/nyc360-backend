using System.ComponentModel.DataAnnotations;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Posts;

namespace NYC360.Domain.Entities.Posts;

public class PostLink
{
    [Key]
    public int PostId { get; set; }       // PK & FK -> Posts.Id
    public Post? Post { get; set; }

    public int LinkedEntityId { get; set; }   // id in target table
    public PostType Type { get; set; }        // duplicate Post.Type for convenience
}