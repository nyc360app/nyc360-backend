using NYC360.Application.Features.Authentication.Commands.Login;
using NYC360.API.Models.Authentication;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Authentication;

public class Login(IMediator mediator) : Endpoint<UserLoginRequest, StandardResponse<LoginResponse>>
{
    public override void Configure()
    {
        Post("/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UserLoginRequest req, CancellationToken ct)
    {
        var command = new LoginCommand(req.Email, req.Password);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}