using NYC360.Application.Features.Roles.Queries.ById;
using NYC360.API.Models.Roles;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Dtos;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Roles;

public class GetRoleByIdEndpoint(IMediator mediator) 
    : Endpoint<GetRoleByIdRequest, StandardResponse<RoleDto>>
{
    public override void Configure()
    {
        Get("/roles-dashboard/{RoleId}");
        Permissions(Domain.Constants.Permissions.Roles.View);
    }

    public override async Task HandleAsync(GetRoleByIdRequest req, CancellationToken ct)
    {
        var query = new GetRoleByIdQuery(req.RoleId);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}