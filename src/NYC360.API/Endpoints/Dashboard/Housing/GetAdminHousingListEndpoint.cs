using NYC360.Application.Features.Housing.Queries.GetAdminList;
using NYC360.Domain.Dtos.Housing;
using NYC360.API.Models.Housing;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Dashboard.Housing;

public class GetAdminHousingListEndpoint(IMediator mediator) 
    : Endpoint<GetAdminHousingListRequest, PagedResponse<AgentListingDto>>
{
    public override void Configure()
    {
        Get("/housing-dashboard/list");
        Permissions(Domain.Constants.Permissions.Housing.View);
    }

    public override async Task HandleAsync(GetAdminHousingListRequest req, CancellationToken ct)
    {
        var query = new GetAdminHousingListQuery(
            req.PageNumber,
            req.PageSize,
            req.IsPublished,
            req.Search
        );

        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, cancellation: ct);
    }
}
