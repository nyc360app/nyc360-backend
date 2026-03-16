using NYC360.Application.Features.Posts.Commands.Share;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using NYC360.API.Models.Post;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Posts;

public class PostShareEndpoint(IMediator mediator) : Endpoint<PostShareRequest, StandardResponse<PostDto>>
{
    public override void Configure()
    {
        Post("/posts/{Id}/share");
    }

    public override async Task HandleAsync(PostShareRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new PostShareCommand(userId.Value, req.Id, req.Commentary);
        var result = await mediator.Send(command, ct);

        await Send.OkAsync(result, ct);
    }
}