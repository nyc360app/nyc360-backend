using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.SpaceListings;
using NYC360.Application.Features.SpaceListings.Commands.Review;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.SpaceListings;

public class ReviewSpaceListingEndpoint(IMediator mediator)
    : Endpoint<ReviewSpaceListingRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/space-dashboard/listings/review");
        Permissions(Domain.Constants.Permissions.SpaceListings.Approve);
    }

    public override async Task HandleAsync(ReviewSpaceListingRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new ReviewSpaceListingCommand(req.ListingId, userId.Value, req.Decision, req.ModerationNote);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}
