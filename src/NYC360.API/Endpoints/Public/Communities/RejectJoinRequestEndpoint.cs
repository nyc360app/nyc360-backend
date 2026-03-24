using NYC360.Application.Features.Communities.Commands.ProcessJoinRequest;
using NYC360.API.Models.Communities;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Communities;

public class RejectJoinRequestEndpoint(IMediator mediator) : Endpoint<ProcessJoinRequestRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/communities/{CommunityId}/requests/{TargetUserId}/reject");
    }

    public override async Task HandleAsync(ProcessJoinRequestRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var result = await mediator.Send(new ProcessJoinRequestCommand(
            userId.Value, req.CommunityId, req.TargetUserId, false), ct);
        await Send.OkAsync(result, ct);
    }
}
