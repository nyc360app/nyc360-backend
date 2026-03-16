using NYC360.Application.Features.Roles.Commands.Edit;
using NYC360.API.Models.Roles;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Roles;

public class EditEndpoint(IMediator mediator) : Endpoint<EditRoleRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/roles-dashboard/edit/{RoleId}");
        Permissions(Domain.Constants.Permissions.Roles.Edit);
    }

    public override async Task HandleAsync(EditRoleRequest req, CancellationToken ct)
    {
        var command = new EditRoleCommand(req.RoleId, req.Name, req.Permissions, req.ContentLimit);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}