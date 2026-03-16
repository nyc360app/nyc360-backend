using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Communities.Dashboard.GetCommunityLeaders;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Communities.Dashboard;

public class GetCommunityLeadersEndpoint(IMediator mediator)
    : EndpointWithoutRequest<StandardResponse<PagedResponse<CommunityMemberDto>>>
{
    public override void Configure()
    {
        Get("/communities-dashboard/{CommunityId}/leaders");
        Roles("SuccessAdmin", "SuperAdmin");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var communityId = Route<int>("CommunityId");
        var page = Query<int>("page", false);
        if (page == 0) page = 1;
        var pageSize = Query<int>("pageSize", false);
        if (pageSize == 0) pageSize = 10;
        
        var query = new GetCommunityLeadersQuery(communityId, page, pageSize);
        var result = await mediator.Send(query, ct);

        await Send.OkAsync(result, ct);
    }
}
