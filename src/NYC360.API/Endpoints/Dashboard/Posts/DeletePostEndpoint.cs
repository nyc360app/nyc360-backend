using NYC360.Application.Features.Posts.Commands.DashboardDelete;
using NYC360.API.Models.Post;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Posts;

public class DeletePostEndpoint(IMediator mediator)
    : Endpoint<PostDeleteDashboardRequest, StandardResponse>
{
    public override void Configure()
    {
        Delete("/posts-dashboard/delete");
        Permissions(Domain.Constants.Permissions.Posts.Delete);
    }

    public override async Task HandleAsync(PostDeleteDashboardRequest req, CancellationToken ct)
    {
        var cmd = new PostDeleteAdminCommand(req.PostId);
        var result = await mediator.Send(cmd, ct);
        await Send.OkAsync(result, ct);
    }
}