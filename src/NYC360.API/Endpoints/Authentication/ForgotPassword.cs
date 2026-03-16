using NYC360.Application.Features.Authentication.Commands.ForgotPassword;
using NYC360.API.Models.Authentication;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Authentication;

public class ForgotPassword(IMediator mediator) : Endpoint<ForgotPasswordRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/auth/forgot-password");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ForgotPasswordRequest req, CancellationToken ct)
    {
        var command = new ForgotPasswordCommand(req.Email);
        var result = await mediator.Send(command, ct);

        // TODO: send the reset token via email here

        await Send.OkAsync(result, ct);
    }
}