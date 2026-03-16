using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Communities.Dashboard.GetAllCommunities;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Communities.Dashboard;

public class GetAllCommunitiesEndpoint(IMediator mediator)
    : EndpointWithoutRequest<StandardResponse<PagedResponse<CommunityDashboardDto>>>
{
    public override void Configure()
    {
        Get("/communities-dashboard/list");
        Roles("SuccessAdmin", "SuperAdmin");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        // Manual binding for query params
        var page = Query<int>("page", false);
        if (page == 0) page = 1;
        var pageSize = Query<int>("pageSize", false);
        if (pageSize == 0) pageSize = 10;
        
        var searchTerm = Query<string>("searchTerm", false);
        var type = Query<CommunityType?>("type", false);
        var locationId = Query<int?>("locationId", false);
        var hasDisbandRequest = Query<bool?>("hasDisbandRequest", false);

        var query = new GetAllCommunitiesQuery(page, pageSize, searchTerm, type, locationId, hasDisbandRequest);
        var result = await mediator.Send(query, ct);

        await Send.OkAsync(result, ct);
    }
}
