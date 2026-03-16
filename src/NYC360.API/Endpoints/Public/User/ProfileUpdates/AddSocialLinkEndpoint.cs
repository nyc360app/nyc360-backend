using NYC360.Application.Features.Users.Commands.ProfileUpdates.AddSocialLink;
using NYC360.API.Models.Users.ProfileUpdate;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.User.ProfileUpdates;

public class AddSocialLinkEndpoint(IMediator mediator) : Endpoint<AddSocialLinkRequest, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/users/profile/social-links");
    }

    public override async Task HandleAsync(AddSocialLinkRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct); 
            return;
        }
        var command = new AddSocialLinkCommand(userId.Value, req.Platform, req.Url);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}