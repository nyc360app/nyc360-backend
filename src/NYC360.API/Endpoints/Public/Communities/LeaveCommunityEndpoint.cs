using NYC360.Application.Features.Communities.Commands.Leave;
using NYC360.API.Models.Communities;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Communities;

public class LeaveCommunityEndpoint(IMediator mediator) : Endpoint<LeaveCommunityRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/communities/leave");
    }
    
    public override async Task HandleAsync(LeaveCommunityRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.OkAsync(StandardResponse.Failure(
                new ApiError("auth.invalidEmail", "Email not found")
            ), ct);
            return;
        }
        
        var command = new LeaveCommunityCommand(userId.Value, req.CommunityId);
        var result = await mediator.Send(command, ct);

        await Send.OkAsync(result, ct);
    }
}