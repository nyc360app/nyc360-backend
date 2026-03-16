using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Topics;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Topics.Queries.GetTopics;

public class GetTopicsQueryHandler(ITopicRepository topicRepository)
    : IRequestHandler<GetTopicsQuery, StandardResponse<IReadOnlyList<TopicDto>>>
{
    public async Task<StandardResponse<IReadOnlyList<TopicDto>>> Handle(GetTopicsQuery request, CancellationToken cancellationToken)
    {
        var topics = await topicRepository.GetTopicsByCategoryAsync(request.Category);
        
        var dtos = topics.Select(t => new TopicDto
        {
            Id = t.Id,
            Name = t.Name,
            Category = t.Category
        }).ToList().AsReadOnly();

        return StandardResponse<IReadOnlyList<TopicDto>>.Success(dtos);
    }
}
