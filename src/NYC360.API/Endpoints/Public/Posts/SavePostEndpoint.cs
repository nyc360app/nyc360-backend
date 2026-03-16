using NYC360.Application.Features.Posts.Commands.SavePost;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;
using System;

namespace NYC360.API.Endpoints.Public.Posts;

public class SavePostEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/posts/{id}/save");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var postId = Route<int>("id");
        var command = new SavePostCommand(userId.Value, postId);
        var result = await mediator.Send(command, ct);
        
        await Send.OkAsync(result, ct);
    }
}