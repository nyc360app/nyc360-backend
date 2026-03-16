using MediatR;
using NYC360.Domain.Dtos.Topics;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Topics.Queries.GetTopicsDashboard;

public class GetTopicsDashboardQuery : IRequest<PagedResponse<TopicDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public Category? Category { get; set; }
}
