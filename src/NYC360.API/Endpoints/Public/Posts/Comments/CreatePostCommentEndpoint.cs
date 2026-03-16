using NYC360.Application.Features.PostComments.Commands.Create;
using NYC360.API.Models.Post.Comments;
using NYC360.Domain.Dtos.Posts;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Posts.Comments;

public class CreatePostCommentEndpoint(IMediator mediator) : Endpoint<PostCommentCreateRequest, StandardResponse<PostCommentDto>>
{
    public override void Configure()
    {
        Post("/posts/comment");
        Permissions(Domain.Constants.Permissions.Posts.Comment);
    }

    public override async Task HandleAsync(PostCommentCreateRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId is null)
        {
            await Send.OkAsync(StandardResponse<PostCommentDto>.Failure(
                new("auth.invalidEmail", "Email not found")
            ), ct);
            return;
        }

        var cmd = new CreatePostCommentCommand(
            userId.Value,
            req.PostId,
            req.ParentCommentId,
            req.Content
        );

        var result = await mediator.Send(cmd, ct);
        await Send.OkAsync(result, ct);
    }
}