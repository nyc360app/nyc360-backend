using NYC360.Application.Features.Verifications.Commands.SubmitIdentity;
using NYC360.API.Models.Users;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.User;

public class SubmitIdentityVerificationEndpoint(IMediator mediator) 
    : Endpoint<IdentityVerificationRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/verifications/identity/submit");
        Roles("Resident", "Organization", "Business");
        AllowFileUploads();
    }

    public override async Task HandleAsync(IdentityVerificationRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new SubmitIdentityVerificationCommand(
            userId.Value,
            req.DocumentType,
            req.Reason,
            req.File);

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}