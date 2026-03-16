using NYC360.Application.Features.Users.Commands.Roles.UpdateRoles;
using NYC360.API.Models.Users;
using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Users;

public class UpdateUserRolesEndpoint(IMediator mediator) : Endpoint<UpdateUserRolesRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/users-dashboard/{UserId}/roles");
        Permissions(Domain.Constants.Permissions.Users.UpdateRoles);
    }

    public override async Task HandleAsync(UpdateUserRolesRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.OkAsync(StandardResponse.Failure(
                new ApiError("auth.unauthorized", "You are not authorized to perform this action")
                ), ct);
            return;
        }

        var userRole = User.GetRole();
        var command = new UpdateUserRolesCommand(req.UserId, req.RoleName, userId.Value, userRole!);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}