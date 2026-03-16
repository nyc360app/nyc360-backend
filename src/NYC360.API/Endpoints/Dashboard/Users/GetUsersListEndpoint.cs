using NYC360.Application.Features.Users.Queries.List;
using NYC360.API.Models.Users;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Users;

public class GetUsersListEndpoint(IMediator mediator)
    : Endpoint<GetUsersListRequest, PagedResponse<UserDashboardDetailDto>>
{
    public override void Configure()
    {
        Get("/users-dashboard/all");
        Permissions(Domain.Constants.Permissions.Users.View);
    }

    public override async Task HandleAsync(GetUsersListRequest req, CancellationToken ct)
    {
        var query = new GetUsersListQuery(req.Page, req.PageSize, req.Search);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, cancellation: ct);
    }
}