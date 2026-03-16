using NYC360.Application.Features.Communities.Commands.Disband;
using NYC360.API.Models.Communities;
using NYC360.API.Extensions;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Communities;

public class DisbandCommunityEndpoint(IMediator mediator) : Endpoint<DisbandCommunityRequest, StandardResponse<string>>
{
    public override void Configure()
    {
        Delete("/communities/{CommunityId}/disband");
    }

    public override async Task HandleAsync(DisbandCommunityRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.OkAsync(StandardResponse<string>.Failure(
                new ApiError("auth.unauthorized", "User not authenticated.")
            ), ct);
            return;
        }

        var command = new DisbandCommunityCommand(
            userId.Value,
            req.CommunityId,
            req.Reason
        );

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}
