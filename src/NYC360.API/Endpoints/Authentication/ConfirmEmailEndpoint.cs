using NYC360.Application.Features.Authentication.Commands.EmailConfirmation;
using NYC360.API.Models.Authentication;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Authentication;

public class ConfirmEmailEndpoint(IMediator mediator) : Endpoint<EmailConfirmationRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/auth/confirm-email");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmailConfirmationRequest req, CancellationToken ct)
    {
        var command = new ConfirmEmailCommand(req.Email, req.Token);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}