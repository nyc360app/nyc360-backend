using FastEndpoints;
using MediatR;
using NYC360.API.Models.Forums;
using NYC360.Application.Features.Forums.Commands.UpdateForum;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Forums;

public class UpdateForumEndpoint(IMediator mediator) : Endpoint<UpdateForumRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/forums-dashboard/update");
        Permissions(Domain.Constants.Permissions.Forums.Edit);
        AllowFileUploads();
    }

    public override async Task HandleAsync(UpdateForumRequest request, CancellationToken ct)
    {
        var command = new UpdateForumCommand(
            request.Id,
            request.Title,
            request.Slug,
            request.Description,
            request.IconFile,
            request.IsActive
        );

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}
