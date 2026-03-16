namespace NYC360.Domain.Entities.Posts;

public class PostAttachment
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public int PostId { get; set; }
    public Post? Post { get; set; }
}