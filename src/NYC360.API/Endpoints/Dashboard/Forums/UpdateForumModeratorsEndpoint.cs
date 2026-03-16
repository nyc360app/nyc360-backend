using FastEndpoints;
using MediatR;
using NYC360.API.Models.Forums;
using NYC360.Application.Features.Forums.Commands.UpdateForumModerators;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Forums;

public class UpdateForumModeratorsEndpoint(IMediator mediator) : Endpoint<UpdateForumModeratorsRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/forums-dashboard/moderators");
        Permissions(Domain.Constants.Permissions.Forums.Edit);
    }

    public override async Task HandleAsync(UpdateForumModeratorsRequest request, CancellationToken ct)
    {
        var command = new UpdateForumModeratorsCommand(
            request.ForumId,
            request.ModeratorIds
        );

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct); 
    }
}
