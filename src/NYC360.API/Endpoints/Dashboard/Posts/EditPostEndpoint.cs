using NYC360.Application.Features.Posts.Commands.DashboardEdit;
using NYC360.API.Models.Post;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Posts;

public class EditPostEndpoint(IMediator mediator) 
    : Endpoint<PostUpdateDashboardRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/posts-dashboard/edit");
        Permissions(Domain.Constants.Permissions.Posts.Edit);
        AllowFileUploads();
    }

    public override async Task HandleAsync(PostUpdateDashboardRequest req, CancellationToken ct)
    {
        var command = new PostUpdateDashboardCommand(
            req.PostId, 
            req.Title, 
            req.Content, 
            req.Category, 
            req.TopicId,
            req.AddedAttachments, 
            req.RemovedAttachments
        );
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}