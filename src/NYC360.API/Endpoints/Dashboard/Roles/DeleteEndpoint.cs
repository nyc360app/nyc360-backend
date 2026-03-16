using NYC360.Application.Features.Roles.Commands.Delete;
using NYC360.API.Models.Roles;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Roles;

public class DeleteRoleEndpoint(IMediator mediator) : Endpoint<DeleteRoleRequest, StandardResponse>
{
    public override void Configure()
    {
        Delete("/roles-dashboard/delete/{RoleId}");
        Permissions(Domain.Constants.Permissions.Roles.Delete);
    }

    public override async Task HandleAsync(DeleteRoleRequest req, CancellationToken ct)
    {
        var command = new DeleteRoleCommand(req.RoleId);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}