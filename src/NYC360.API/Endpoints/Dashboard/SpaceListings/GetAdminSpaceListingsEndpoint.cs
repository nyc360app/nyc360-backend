using FastEndpoints;
using MediatR;
using NYC360.API.Models.SpaceListings;
using NYC360.Application.Features.SpaceListings.Queries.Admin;
using NYC360.Domain.Dtos.SpaceListings;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.SpaceListings;

public class GetAdminSpaceListingsEndpoint(IMediator mediator)
    : Endpoint<SpaceListingsListRequest, PagedResponse<SpaceListingListItemDto>>
{
    public override void Configure()
    {
        Get("/space-dashboard/listings/pending");
        Permissions(Domain.Constants.Permissions.SpaceListings.Review);
    }

    public override async Task HandleAsync(SpaceListingsListRequest req, CancellationToken ct)
    {
        var query = new GetAdminSpaceListingsQuery(
            req.Page,
            req.PageSize,
            req.Department,
            req.EntityType,
            req.Status,
            req.Search);

        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}
