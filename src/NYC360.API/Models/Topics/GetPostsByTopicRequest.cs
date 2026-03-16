namespace NYC360.API.Models.Topics;

public class GetPostsByTopicRequest
{
    public int TopicId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
