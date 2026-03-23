using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.SpaceListings;
using NYC360.Application.Features.SpaceListings.Commands.Assign;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.SpaceListings;

public class AssignSpaceListingEndpoint(IMediator mediator)
    : Endpoint<AssignSpaceListingRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/space-dashboard/listings/assign");
        Permissions(Domain.Constants.Permissions.SpaceListings.Assign);
    }

    public override async Task HandleAsync(AssignSpaceListingRequest req, CancellationToken ct)
    {
        var adminId = User.GetId();
        if (adminId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new AssignSpaceListingCommand(req.ListingId, req.ReviewerUserId, adminId.Value);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}
