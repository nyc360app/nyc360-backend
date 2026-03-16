using NYC360.Application.Features.Posts.Commands.UserEdit;
using NYC360.API.Models.Post;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Posts;

public class EditOwnPostEndpoint(IMediator mediator)
    : Endpoint<PostUpdateUserRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/posts/edit");
        AllowFileUploads();
    }

    public override async Task HandleAsync(PostUpdateUserRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId is null)
        {
            await Send.OkAsync(StandardResponse.Failure(
                new("auth.invalidEmail", "Email not found")
            ), ct);
            return;
        }

        var cmd = new PostUpdateUserCommand(
            userId.Value,
            User.GetRole()!,
            req.PostId,
            req.Title,
            req.Content,
            req.Category,
            req.TopicId,
            req.TagIds,
            req.AddedAttachments,
            req.RemovedAttachments
        );

        var result = await mediator.Send(cmd, ct);
        await Send.OkAsync(result, ct);
    }
}