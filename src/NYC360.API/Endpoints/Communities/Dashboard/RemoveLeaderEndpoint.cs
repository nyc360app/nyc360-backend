using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.Application.Features.Communities.Dashboard.RemoveLeader;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Communities.Dashboard;

public class RemoveLeaderEndpoint(IMediator mediator)
    : EndpointWithoutRequest<StandardResponse<string>>
{
    public override void Configure()
    {
        Delete("/communities-dashboard/{CommunityId}/leaders/{UserId}");
        Roles("SuccessAdmin", "SuperAdmin");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var communityId = Route<int>("CommunityId");
        var userId = Route<int>("UserId");
        var adminUserId = User.GetId();

        var command = new RemoveLeaderCommand(communityId, userId, adminUserId.Value);
        var result = await mediator.Send(command, ct);

        await Send.OkAsync(result, ct);
    }
}
