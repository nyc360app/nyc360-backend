using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Topics.Queries.GetTopicsDashboard;
using NYC360.Domain.Dtos.Topics;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Topics;

public class GetTopicsDashboardEndpoint(IMediator mediator) : Endpoint<GetTopicsDashboardQuery, PagedResponse<TopicDto>>
{
    public override void Configure()
    {
        Get("/topics-dashboard/list");
        Permissions(Domain.Constants.Permissions.Topics.View);
    }

    public override async Task HandleAsync(GetTopicsDashboardQuery req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await Send.OkAsync(result, ct);
    }
}
