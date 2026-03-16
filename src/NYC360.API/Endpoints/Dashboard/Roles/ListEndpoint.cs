using NYC360.Application.Features.Roles.Queries.List;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Dtos;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Roles;

public class ListEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse<List<RoleDto>>>
{
    public override void Configure()
    {
        Get("/roles-dashboard/all");
        Permissions(Domain.Constants.Permissions.Roles.View);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetRolesQuery();
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}