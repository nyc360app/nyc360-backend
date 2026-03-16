using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.Application.Features.Communities.Dashboard.AssignLeader;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Communities.Dashboard;

public class AssignLeaderRequest
{
    public int UserId { get; set; }
}

public class AssignLeaderEndpoint(IMediator mediator)
    : Endpoint<AssignLeaderRequest, StandardResponse<string>>
{
    public override void Configure()
    {
        Post("/communities-dashboard/{CommunityId}/assign-leader");
        Roles("Admin", "SuperAdmin");
    }

    public override async Task HandleAsync(AssignLeaderRequest req, CancellationToken ct)
    {
        var communityId = Route<int>("CommunityId");
        var adminUserId = User.GetId();

        var command = new AssignLeaderCommand(communityId, req.UserId, adminUserId.Value);
        var result = await mediator.Send(command, ct);

        await Send.OkAsync(result, ct);
    }
}
