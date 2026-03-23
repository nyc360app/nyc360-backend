using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.Application.Features.SpaceListings.Commands.Resubmit;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.SpaceListings;

public class ResubmitSpaceListingEndpoint(IMediator mediator)
    : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Post("/space/listings/resubmit/{id}");
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
        var result = await mediator.Send(new ResubmitSpaceListingCommand(id, userId.Value), ct);
        await Send.OkAsync(result, ct);
    }
}
