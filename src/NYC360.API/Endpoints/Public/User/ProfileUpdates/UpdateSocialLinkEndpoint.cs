using NYC360.Application.Features.Users.Commands.ProfileUpdates.UpdateSocialLink;
using NYC360.API.Models.Users.ProfileUpdate;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.User.ProfileUpdates;

public class UpdateSocialLinkEndpoint(IMediator mediator) : Endpoint<UpdateSocialLinkRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/users/profile/social-links");
    }

    public override async Task HandleAsync(UpdateSocialLinkRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new UpdateSocialLinkCommand(
            userId.Value, 
            req.LinkId, 
            req.Platform, 
            req.Url);

        var result = await mediator.Send(command, ct);
        
        await Send.OkAsync(result, ct);
    }
}