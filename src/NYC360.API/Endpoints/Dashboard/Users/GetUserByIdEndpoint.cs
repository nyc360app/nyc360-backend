using NYC360.Application.Features.Users.Queries.GetById;
using NYC360.API.Models.Users;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Users;

public class GetUserByIdEndpoint(IMediator mediator)
    : Endpoint<GetUserByIdRequest, StandardResponse<UserDashboardDetailDto>>
{
    public override void Configure()
    {
        Get("/users-dashboard/{Id}");
        Permissions(Domain.Constants.Permissions.Users.View);
    }

    public override async Task HandleAsync(GetUserByIdRequest req, CancellationToken ct)
    {
        var query = new GetUserByIdQuery(req.Id);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, cancellation: ct);
    }
}