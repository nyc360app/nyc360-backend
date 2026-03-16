using NYC360.Application.Features.Authentication.Commands.Logout;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Authentication;

public class LogoutEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Post("/auth/logout");
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.OkAsync(StandardResponse.Failure(new ApiError("auth.usernotfound", "user not found")), ct);
            return;
        }
        
        var command = new LogoutCommand(userId.Value);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}