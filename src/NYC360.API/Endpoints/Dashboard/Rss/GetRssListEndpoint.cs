using NYC360.Application.Features.RssSources.Queries.GetAll;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Rss;

public class GetRssListEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse<List<RssSourceDto>>>
{
    public override void Configure()
    {
        Get("/rss-dashboard/list");
        Permissions(Domain.Constants.Permissions.RssFeeds.View);
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await mediator.Send(new RssSourceGetAllQuery(), ct);
        await Send.OkAsync(result, ct);
    }
}