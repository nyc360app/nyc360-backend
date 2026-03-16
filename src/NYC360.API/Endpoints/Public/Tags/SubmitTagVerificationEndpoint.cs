using NYC360.Application.Features.Verifications.Commands.TagRequest;
using NYC360.Domain.Wrappers;
using NYC360.API.Models.Tags;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Tags;

public class SubmitTagVerificationEndpoint(IMediator mediator): Endpoint<TagVerificationRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/verifications/tag/submit");
        AllowFileUploads();
    }

    public override async Task HandleAsync(TagVerificationRequest req, CancellationToken ct)
    {
        // Get UserId from Claims (assuming an extension method exists)
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new SubmitTagVerificationCommand(
            userId.Value, 
            req.TagId, 
            req.Reason, 
            req.DocumentType, 
            req.File);

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}