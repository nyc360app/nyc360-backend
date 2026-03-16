using NYC360.Application.Features.Communities.Queries.GetMyCommunities;
using NYC360.Domain.Dtos.Communities;
using NYC360.API.Models.Communities;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Communities;

public class GetMyCommunitiesEndpoint(IMediator mediator) : Endpoint<GetDiscoveryRequest, PagedResponse<CommunityDiscoveryDto>>
{
    public override void Configure()
    {
        Get("/communities/my-communities");
    }

    public override async Task HandleAsync(GetDiscoveryRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        var query = new GetMyCommunitiesQuery(
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