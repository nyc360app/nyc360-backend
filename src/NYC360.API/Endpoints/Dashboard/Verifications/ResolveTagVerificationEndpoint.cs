using NYC360.Application.Features.Verifications.Commands.ResolveTagRequest;
using NYC360.API.Models.Tags;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Dashboard.Verifications;

public class ResolveTagVerificationEndpoint(IMediator mediator) 
    : Endpoint<ResolveTagVerificationRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/verifications/tags/resolve");
        Permissions(Domain.Constants.Permissions.Tags.Verify);
    }

    public override async Task HandleAsync(ResolveTagVerificationRequest req, CancellationToken ct)
    {
        var adminId = User.GetId();
        if (adminId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new ResolveTagVerificationCommand(
            adminId.Value,
            req.RequestId, 
            req.Approved, 
            req.AdminComment
        );

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}