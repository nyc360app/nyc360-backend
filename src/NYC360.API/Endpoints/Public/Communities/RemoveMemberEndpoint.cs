using NYC360.Application.Features.Communities.Commands.RemoveMember;
using NYC360.API.Models.Communities;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Communities;

public class RemoveMemberEndpoint(IMediator mediator) : Endpoint<RemoveMemberRequest, StandardResponse>
{
    public override void Configure()
    {
        Delete("/communities/{CommunityId}/members/{TargetUserId}");
    }

    public override async Task HandleAsync(RemoveMemberRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new RemoveMemberCommand(
            User.GetId()!.Value, 
            req.CommunityId, 
            req.TargetUserId
        ), ct);

        await Send.OkAsync(result, ct);
    }
}