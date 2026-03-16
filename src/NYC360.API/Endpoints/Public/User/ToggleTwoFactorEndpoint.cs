using NYC360.Application.Features.Users.Commands.ToggleTwoFactor;
using NYC360.API.Models.Users;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.User;

public class ToggleTwoFactorEndpoint(IMediator mediator)
    : Endpoint<ToggleTwoFactorRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/users/me/toggle-2fa");
    }

    public override async Task HandleAsync(ToggleTwoFactorRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.OkAsync(StandardResponse.Failure(
                new ApiError("auth.invalidEmail", "Email not found")
                ), ct);
            return;
        }
        var command = new ToggleTwoFactorCommand(userId.Value, req.Enable);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}