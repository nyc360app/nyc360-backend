using MediatR;
using NYC360.Domain.Dtos.Topics;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Topics.Queries.GetTopics;

public class GetTopicsQuery : IRequest<StandardResponse<IReadOnlyList<TopicDto>>>
{
    public Category? Category { get; set; }
}
