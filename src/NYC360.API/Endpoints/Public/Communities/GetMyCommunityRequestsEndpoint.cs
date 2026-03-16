using NYC360.Application.Features.Communities.Queries.GetMyCommunityRequests;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Communities;

public class GetMyCommunityRequestsEndpoint(IMediator mediator) 
    : EndpointWithoutRequest<StandardResponse<MyCommunityRequestsDto>>
{
    public override void Configure()
    {
        Get("/communities/my-requests");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.OkAsync(StandardResponse<MyCommunityRequestsDto>.Failure(
                new ApiError("auth.unauthorized", "User session not found")
            ), ct);
            return;
        }

        var query = new GetMyCommunityRequestsQuery(userId.Value);
        var result = await mediator.Send(query, ct);

        await Send.OkAsync(result, ct);
    }
}
