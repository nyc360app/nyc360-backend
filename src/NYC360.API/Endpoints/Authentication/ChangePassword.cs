using System.Security.Claims;
using NYC360.Application.Features.Authentication.Commands.ChangePassword;
using Microsoft.AspNetCore.Authorization;
using NYC360.API.Models.Authentication;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Authentication;

[Authorize]
public class ChangePassword(IMediator mediator) : Endpoint<ChangePasswordRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/auth/change-password");
    }

    public override async Task HandleAsync(ChangePasswordRequest req, CancellationToken ct)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (email == null)
        {
            await Send.OkAsync(StandardResponse.Failure(
                new ApiError("auth.invalidEmail", "Email not found")
                ), ct);
            return;
        }
        
        var command = new ChangePasswordCommand(email, req.CurrentPassword, req.NewPassword);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}