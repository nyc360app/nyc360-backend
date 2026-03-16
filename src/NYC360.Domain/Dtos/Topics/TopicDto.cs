using NYC360.Domain.Enums;

namespace NYC360.Domain.Dtos.Topics;

public class TopicDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Category? Category { get; set; }
}
