using NYC360.Application.Features.Communities.Queries.GetDiscovery;
using NYC360.Domain.Dtos.Communities;
using NYC360.API.Models.Communities;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;

namespace NYC360.API.Endpoints.Public.Communities;

public class GetCommunityDiscoveryEndpoint(IMediator mediator) : Endpoint<GetDiscoveryRequest, PagedResponse<CommunityDiscoveryDto>>
{
    public override void Configure()
    {
        Get("/communities/discovery");
    }

    public override async Task HandleAsync(GetDiscoveryRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        var query = new GetCommunityDiscoveryQuery(
            userId.Value,
            req.Search, 
            req.Type, 
            req.LocationId, 
            req.Page, 
            req.PageSize
        );
            
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}