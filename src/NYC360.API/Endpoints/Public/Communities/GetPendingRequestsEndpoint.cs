using NYC360.Application.Features.Communities.Queries.GetPendingRequests;
using NYC360.Domain.Dtos.Communities;
using NYC360.API.Models.Communities;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Communities;

public class GetPendingRequestsEndpoint(IMediator mediator) 
    : Endpoint<GetPendingRequestsRequest, StandardResponse<List<CommunityPendingRequestDto>>>
{
    public override void Configure()
    {
        Get("/communities/{CommunityId}/requests");
    }

    public override async Task HandleAsync(GetPendingRequestsRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.OkAsync(StandardResponse<List<CommunityPendingRequestDto>>.Failure(
                new ApiError("auth.unauthorized", "User session not found")
            ), ct);
            return;
        }

        var query = new GetPendingJoinRequestsQuery(userId.Value, req.CommunityId);
        var result = await mediator.Send(query, ct);

        await Send.OkAsync(result, ct);
    }
}