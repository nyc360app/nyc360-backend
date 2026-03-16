using NYC360.Application.Features.Authentication.Commands.TwoFactorVerify;
using NYC360.Application.Features.Authentication.Commands.Login;
using NYC360.API.Models.Authentication;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Authentication;

public class TwoFactorVerifyEndpoint(IMediator mediator)
    : Endpoint<TwoFactorVerifyRequest, StandardResponse<LoginResponse>>
{
    public override void Configure()
    {
        Post("/auth/2fa-verify");
        AllowAnonymous();
    }

    public override async Task HandleAsync(TwoFactorVerifyRequest req, CancellationToken ct)
    {
        var command = new TwoFactorVerifyCommand(req.Email, req.Code);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}