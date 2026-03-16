using NYC360.Application.Features.Authentication.Commands.PasswordReset;
using NYC360.API.Models.Authentication;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Authentication;

public class PasswordReset(IMediator mediator) : Endpoint<PasswordResetRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/auth/password-reset");
        AllowAnonymous();
    }

    public override async Task HandleAsync(PasswordResetRequest req, CancellationToken ct)
    {
        var command = new PasswordResetCommand(req.Email, req.NewPassword, req.Token);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}