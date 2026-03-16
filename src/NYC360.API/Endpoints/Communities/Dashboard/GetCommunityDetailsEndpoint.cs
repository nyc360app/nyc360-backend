using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Communities.Dashboard.GetCommunityDetails;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Communities.Dashboard;

public class GetCommunityDetailsEndpoint(IMediator mediator)
    : EndpointWithoutRequest<StandardResponse<CommunityDetailsDto>>
{
    public override void Configure()
    {
        Get("/communities-dashboard/{CommunityId}");
        Roles("SuccessAdmin", "SuperAdmin");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var communityId = Route<int>("CommunityId");
        
        var query = new GetCommunityDetailsQuery(communityId);
        var result = await mediator.Send(query, ct);

        await Send.OkAsync(result, ct);
    }
}
