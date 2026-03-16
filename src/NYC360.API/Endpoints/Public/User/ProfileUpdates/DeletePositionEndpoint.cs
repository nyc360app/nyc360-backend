using NYC360.Application.Features.Users.Commands.ProfileUpdates.DeletePosition;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.User.ProfileUpdates;

public class DeletePositionEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Delete("/users/profile/positions/{positionId}");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        // Fetch positionId from the route
        var positionId = Route<int>("positionId");

        var command = new DeletePositionCommand(userId.Value, positionId);
        var result = await mediator.Send(command, ct);
        
        await Send.OkAsync(result, ct);
    }
}