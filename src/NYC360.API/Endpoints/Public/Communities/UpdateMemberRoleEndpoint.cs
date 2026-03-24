using NYC360.Application.Features.Communities.Commands.UpdateMemberRole;
using NYC360.API.Models.Communities;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Communities;

public class UpdateMemberRoleEndpoint(IMediator mediator) : Endpoint<UpdateMemberRoleRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/communities/{CommunityId}/members/{TargetUserId}/role");
    }

    public override async Task HandleAsync(UpdateMemberRoleRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var result = await mediator.Send(new UpdateMemberRoleCommand(
            userId.Value,
            req.CommunityId,
            req.TargetUserId,
            req.NewRole
        ), ct);

        await Send.OkAsync(result, ct);
    }
}
