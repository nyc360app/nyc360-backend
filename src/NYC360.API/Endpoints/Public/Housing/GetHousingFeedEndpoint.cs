using NYC360.Application.Features.Housing.Queries.GetFeed;
using NYC360.API.Models.Housing;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Housing;

public class GetHousingFeedEndpoint(IMediator mediator) 
    : Endpoint<GetHousingFeedRequest, PagedResponse<HousingMinimalDto>>
{
    public override void Configure()
    {
        Get("/housing/feed");
    }

    public override async Task HandleAsync(GetHousingFeedRequest req, CancellationToken ct)
    {
        var query = new GetHousingFeedQuery(
            req.Page,
            req.PageSize,
            req.IsRenting,
            req.MinPrice,
            req.MaxPrice,
            req.LocationId,
            req.Search
        );
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, cancellation: ct);
    }
}