using FastEndpoints;
using MediatR;
using NYC360.API.Models.RssSources;
using NYC360.Application.Features.RssFeedItems.Queries.GetLatest;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Rss;

public class GetLatestRssFeedItemsEndpoint(IMediator mediator)
    : Endpoint<GetLatestRssFeedItemsRequest, StandardResponse<List<RssFeedItemDto>>>
{
    public override void Configure()
    {
        Get("/rss-feed/items/latest");
    }

    public override async Task HandleAsync(GetLatestRssFeedItemsRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new GetLatestRssFeedItemsQuery(req.Category, req.Limit), ct);
        await Send.OkAsync(result, ct);
    }
}
