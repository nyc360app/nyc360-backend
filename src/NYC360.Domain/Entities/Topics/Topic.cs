using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Enums;

namespace NYC360.Domain.Entities.Topics;

public class Topic
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Category? Category { get; set; }
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
