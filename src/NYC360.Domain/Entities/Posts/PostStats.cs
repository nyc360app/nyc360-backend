using System.ComponentModel.DataAnnotations;

namespace NYC360.Domain.Entities.Posts;

public class PostStats
{
    [Key]
    public int PostId { get; set; }
    public int Views { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public int Comments { get; set; }
    public int Shares { get; set; }

    public Post Post { get; set; }
}