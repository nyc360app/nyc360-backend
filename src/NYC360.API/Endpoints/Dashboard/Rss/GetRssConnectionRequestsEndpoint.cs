using NYC360.Domain.Enums;
using FastEndpoints;
using MediatR;
using NYC360.Application.Features.RssSources.Queries.GetRequests;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Wrappers;
using NYC360.API.Models.RssSources;

namespace NYC360.API.Endpoints.Dashboard.Rss;

public class GetRssConnectionRequestsEndpoint(IMediator mediator) : Endpoint<GetRssConnectionRequestsListRequest, PagedResponse<RssConnectionRequestDto>>
{
    public override void Configure()
    {
        Get("/rss-dashboard/requests");
        Permissions(Domain.Constants.Permissions.RssFeeds.View);
    }
    
    public override async Task HandleAsync(GetRssConnectionRequestsListRequest req, CancellationToken ct)
    {
        var query = new GetRssConnectionRequestsQuery(req.PageNumber, req.PageSize, req.Status);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}
