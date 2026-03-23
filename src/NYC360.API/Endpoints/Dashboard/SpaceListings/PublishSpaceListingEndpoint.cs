using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.Application.Features.SpaceListings.Commands.Publish;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.SpaceListings;

public class PublishSpaceListingEndpoint(IMediator mediator)
    : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Post("/space-dashboard/listings/{id}/publish");
        Permissions(Domain.Constants.Permissions.SpaceListings.Publish);
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
        var result = await mediator.Send(new PublishSpaceListingCommand(id, userId.Value), ct);
        await Send.OkAsync(result, ct);
    }
}
