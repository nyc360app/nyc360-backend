using NYC360.Application.Features.Professions.Commands.Apply;
using NYC360.API.Models.Professions;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Professions;

public class ApplyToJobEndpoint(IMediator mediator) 
    : Endpoint<ApplyToJobRequest, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/professions/offers/{JobOfferId}/apply");
        AllowFileUploads();
    }

    public override async Task HandleAsync(ApplyToJobRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var command = new ApplyToJobCommand(userId.Value, req.JobOfferId, req.CoverLetter, req.Attachment);
        var result = await mediator.Send(command, ct);

        await Send.OkAsync(result, ct);
    }
}