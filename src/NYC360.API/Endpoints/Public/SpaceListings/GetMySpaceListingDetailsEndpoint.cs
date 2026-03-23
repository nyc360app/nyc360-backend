using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.Application.Features.SpaceListings.Queries.GetMyListingDetails;
using NYC360.Domain.Dtos.SpaceListings;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.SpaceListings;

public class GetMySpaceListingDetailsEndpoint(IMediator mediator)
    : EndpointWithoutRequest<StandardResponse<SpaceListingDetailsDto>>
{
    public override void Configure()
    {
        Get("/space/listings/{id}");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var id = Route<int>("id");
        var result = await mediator.Send(new GetMySpaceListingDetailsQuery(id, userId.Value), ct);
        await Send.OkAsync(result, ct);
    }
}
