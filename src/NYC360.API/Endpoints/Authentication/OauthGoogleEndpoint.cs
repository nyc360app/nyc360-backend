using NYC360.Application.Features.Authentication.Commands.OAuthGoogle;
using NYC360.Application.Features.Authentication.Commands.Login;
using NYC360.API.Models.Authentication;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Authentication;

public class OauthGoogleEndpoint(IMediator mediator) : Endpoint<OAuthLoginRequest, StandardResponse<LoginResponse>>
{
    public override void Configure()
    {
        Post("/oauth/google");
        AllowAnonymous();
    }

    public override async Task HandleAsync(OAuthLoginRequest req, CancellationToken ct)
    {
        var command = new OAuthLoginGoogleCommand(req.IdToken);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}