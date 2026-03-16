using NYC360.Application.Features.Users.Queries.GetUserInfo;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.User;

public class GetUserInfo(IMediator mediator) : EndpointWithoutRequest<StandardResponse<UserInfoDto>>
{
    public override void Configure()
    {
        Get("/users/my-info");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var query = new GetUserInfoQuery(userId.Value);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}