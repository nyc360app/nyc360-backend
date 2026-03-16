using NYC360.Application.Features.Users.Commands.ProfileUpdates.UpdatePosition;
using NYC360.API.Models.Users.ProfileUpdate;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.User.ProfileUpdates;

public class UpdatePositionEndpoint(IMediator mediator) : Endpoint<UpdatePositionRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/users/profile/positions");
    }

    public override async Task HandleAsync(UpdatePositionRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new UpdatePositionCommand(
            userId.Value,
            req.PositionId,
            req.Title,
            req.Company,
            req.StartDate,
            req.EndDate,
            req.IsCurrent);

        var result = await mediator.Send(command, ct);
        
        await Send.OkAsync(result, ct);
    }
}