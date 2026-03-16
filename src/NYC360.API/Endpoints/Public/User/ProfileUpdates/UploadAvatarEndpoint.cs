using NYC360.Application.Features.Users.Commands.ProfileUpdates.UploadAvatar;
using NYC360.API.Models.Users.ProfileUpdate;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.User.ProfileUpdates;

public class UploadAvatarEndpoint(IMediator mediator): Endpoint<UploadAvatarRequest, StandardResponse<string>>
{
    public override void Configure()
    {
        Post("/users/profile/avatar");
        AllowFileUploads();
        Summary(s => {
            s.Description = "Upload or update the user's profile picture.";
            s.RequestParam(r => r.Avatar, "The image file to upload (Max 5MB).");
        });
    }

    public override async Task HandleAsync(UploadAvatarRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new UploadAvatarCommand(userId.Value, req.Avatar);
        var result = await mediator.Send(command, ct);
        
        await Send.OkAsync(result, ct);
    }
}