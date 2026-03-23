using FastEndpoints;
using MediatR;
using NYC360.Application.Features.SpaceListings.Queries.Admin;
using NYC360.Domain.Dtos.SpaceListings;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.SpaceListings;

public class GetAdminSpaceListingDetailsEndpoint(IMediator mediator)
    : EndpointWithoutRequest<StandardResponse<SpaceListingDetailsDto>>
{
    public override void Configure()
    {
        Get("/space-dashboard/listings/{id}");
        Permissions(Domain.Constants.Permissions.SpaceListings.Review);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var result = await mediator.Send(new GetAdminSpaceListingDetailsQuery(id), ct);
        await Send.OkAsync(result, ct);
    }
}
