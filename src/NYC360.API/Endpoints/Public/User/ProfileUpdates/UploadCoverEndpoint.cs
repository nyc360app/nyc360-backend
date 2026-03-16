using NYC360.Application.Features.Users.Commands.ProfileUpdates.UploadCover;
using NYC360.API.Models.Users.ProfileUpdate;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.User.ProfileUpdates;

public class UploadCoverEndpoint(IMediator mediator): Endpoint<UploadCoverRequest, StandardResponse<string>>
{
    public override void Configure()
    {
        Post("/users/profile/cover");
        AllowFileUploads();
        Summary(s => {
            s.Description = "Upload or update the user's profile cover/header image.";
            s.RequestParam(r => r.Cover, "The image file to upload (Max 10MB).");
        });
    }

    public override async Task HandleAsync(UploadCoverRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new UploadCoverCommand(userId.Value, req.Cover);
        var result = await mediator.Send(command, ct);
        
        await Send.OkAsync(result, ct);
    }
}