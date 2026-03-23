using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.SpaceListings;
using NYC360.Application.Features.SpaceListings.Queries.GetMyListings;
using NYC360.Domain.Dtos.SpaceListings;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.SpaceListings;

public class GetMySpaceListingsEndpoint(IMediator mediator)
    : Endpoint<SpaceListingsListRequest, PagedResponse<SpaceListingListItemDto>>
{
    public override void Configure()
    {
        Get("/space/listings/mine");
    }

    public override async Task HandleAsync(SpaceListingsListRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var query = new GetMySpaceListingsQuery(userId.Value, req.Page, req.PageSize);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}
