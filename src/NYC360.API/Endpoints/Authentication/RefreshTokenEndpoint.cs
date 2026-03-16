using NYC360.Application.Features.Authentication.Commands.RefreshToken;
using NYC360.Application.Features.Authentication.Commands.Login;
using NYC360.API.Models.Authentication;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Authentication;

public class RefreshTokenEndpoint(IMediator mediator) : Endpoint<RefreshTokenRequest, StandardResponse<LoginResponse>>
{
    public override void Configure()
    {
        Post("/auth/refresh-token");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RefreshTokenRequest req, CancellationToken ct)
    {
        var command = new RefreshTokenCommand(req.Token);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}