using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.Post;
using NYC360.Application.Features.Posts.Commands.FeatureToggle;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Posts;

public sealed class FeaturePostEndpoint(IMediator mediator)
    : Endpoint<PostFeatureToggleRequest, StandardResponse<PostFeatureStatusDto>>
{
    public override void Configure()
    {
        Post("/posts/{id:int}/feature");
        Roles("Admin", "SuperAdmin");
    }

    public override async Task HandleAsync(PostFeatureToggleRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId is null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var postId = Route<int>("id");
        var command = new PostFeatureToggleCommand(postId, req.IsFeatured, userId.Value);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}
