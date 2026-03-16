using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Topics;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Topics.Queries.GetTopicsDashboard;

public class GetTopicsDashboardQueryHandler(ITopicRepository topicRepository)
    : IRequestHandler<GetTopicsDashboardQuery, PagedResponse<TopicDto>>
{
    public async Task<PagedResponse<TopicDto>> Handle(GetTopicsDashboardQuery request, CancellationToken cancellationToken)
    {
        var (topics, total) = await topicRepository.GetPagedTopicsAsync(request.Page, request.PageSize, request.Search, request.Category);

        var dtos = topics.Select(t => new TopicDto
        {
            Id = t.Id,
            Name = t.Name,
            Category = t.Category
        }).ToList();

        return PagedResponse<TopicDto>.Create(dtos, total, request.Page, request.PageSize);
    }
}
