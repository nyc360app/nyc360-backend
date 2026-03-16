using NYC360.Application.Features.Posts.Commands.UserDelete;
using NYC360.API.Models.Post;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Posts;

public class DeleteOwnPostEndpoint(IMediator mediator) 
    : Endpoint<PostDeleteUserRequest, StandardResponse>
{
    public override void Configure()
    {
        Delete("/posts/delete/{PostId:int}");
        Permissions(Domain.Constants.Permissions.Posts.Create);
    }

    public override async Task HandleAsync(PostDeleteUserRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId is null)
        {
            await Send.OkAsync(StandardResponse.Failure(
                new("auth.invalidEmail", "Email not found")
            ), ct);
            return;
        }

        var cmd = new PostDeleteUserCommand(userId.Value, req.PostId);
        var result = await mediator.Send(cmd, ct);
        await Send.OkAsync(result, ct);
    }
}