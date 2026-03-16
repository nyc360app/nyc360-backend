using MediatR;
using NYC360.API.Models.Users.ProfileUpdate;
using NYC360.Application.Features.Users.Commands.ProfileUpdates.AddPosition;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using NYC360.API.Extensions;

namespace NYC360.API.Endpoints.Public.User.ProfileUpdates;

public class AddPositionEndpoint(IMediator mediator) : Endpoint<AddPositionRequest, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/users/profile/positions");
    }

    public override async Task HandleAsync(AddPositionRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
        }
        var command = new AddPositionCommand(
            userId!.Value,
            req.Title,
            req.Company,
            req.StartDate,
            req.EndDate,
            req.IsCurrent);

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}