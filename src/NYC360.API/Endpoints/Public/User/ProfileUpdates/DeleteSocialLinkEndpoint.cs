using NYC360.Application.Features.Users.Commands.ProfileUpdates.DeleteSocialLink;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.User.ProfileUpdates;

public class DeleteSocialLinkEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Delete("/users/profile/social-links/{linkId}");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        // Extract ID from the URL route
        var linkId = Route<int>("linkId");

        var command = new DeleteSocialLinkCommand(userId.Value, linkId);
        var result = await mediator.Send(command, ct);
        
        await Send.OkAsync(result, ct);
    }
}