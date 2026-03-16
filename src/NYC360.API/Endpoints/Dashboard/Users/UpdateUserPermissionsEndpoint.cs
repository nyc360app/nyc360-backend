using NYC360.Application.Features.Users.Commands.Roles.UpdatePermissions;
using NYC360.API.Models.Users;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Users;

public class UpdateUserPermissionsEndpoint(IMediator mediator) : Endpoint<UpdateUserPermissionsRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/users-dashboard/{UserId}/permissions");
        Permissions(Domain.Constants.Permissions.Users.UpdatePermissions);
    }

    public override async Task HandleAsync(UpdateUserPermissionsRequest req, CancellationToken ct)
    {
        var command = new UpdateUserPermissionsCommand(req.UserId, req.Permissions);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}