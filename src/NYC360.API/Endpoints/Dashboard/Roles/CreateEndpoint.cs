using NYC360.Application.Features.Roles.Commands.Create;
using NYC360.API.Models.Roles;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Roles;

public class CreateEndpoint(IMediator mediator) : Endpoint<CreateRoleRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/roles-dashboard/create");
        Permissions(Domain.Constants.Permissions.Roles.Create);
    }

    public override async Task HandleAsync(CreateRoleRequest req, CancellationToken ct)
    {
        var command = new CreateRoleCommand(req.Name, req.Permissions, req.ContentLimit);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}