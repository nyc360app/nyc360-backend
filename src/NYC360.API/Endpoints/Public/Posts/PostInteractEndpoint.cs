using NYC360.Application.Features.Posts.Commands.Interaction;
using NYC360.API.Models.Post;
using NYC360.API.Extensions;
using NYC360.Domain.Enums;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Posts;

public class PostInteractEndpoint(IMediator mediator) : Endpoint<PostInteractionRequest, StandardResponse<InteractionType?>>
{
    public override void Configure()
    {
        Put("/posts/{PostId}/interact");
        Permissions(Domain.Constants.Permissions.Posts.Interact);
    }
    
    public override async Task HandleAsync(PostInteractionRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId is null)
        {
            await Send.OkAsync(StandardResponse<InteractionType?>.Failure(
                new ApiError("auth.invalidEmail", "Email not found")
                ), ct);
            return;
        }
        var command = new PostInteractionCommand(userId.Value, req.PostId, req.Type);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}