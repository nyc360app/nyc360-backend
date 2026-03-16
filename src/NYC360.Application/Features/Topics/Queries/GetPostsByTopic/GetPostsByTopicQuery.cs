using MediatR;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Topics.Queries.GetPostsByTopic;

public class GetPostsByTopicQuery : IRequest<PagedResponse<PostDto>>
{
    public int? UserId { get; set; }
    public int TopicId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
