using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Queries.GetPendingRequests;

public class GetPendingJoinRequestsQueryHandler(
    ICommunityRepository communityRepository,
    ICommunityPermissionService permissionService)
    : IRequestHandler<GetPendingJoinRequestsQuery, StandardResponse<List<CommunityPendingRequestDto>>>
{
    public async Task<StandardResponse<List<CommunityPendingRequestDto>>> Handle(GetPendingJoinRequestsQuery request, CancellationToken ct)
    {
        // 1. Authorization
        if (!await permissionService.IsLeaderAsync(request.UserId, request.CommunityId, ct))
        {
            return StandardResponse<List<CommunityPendingRequestDto>>.Failure(
                new ApiError("auth.unauthorized", "Only the owner can view pending requests."));
        }

        // 2. Fetch Requests (Requires an update to the Repository)
        var requests = await communityRepository.GetPendingRequestsAsync(request.CommunityId, ct);

        // 3. Map to DTO
        var dtos = requests.Select(r => new CommunityPendingRequestDto(
            r.UserId,
            r.User?.GetFullName() ?? "Unknown User",
            r.User!.AvatarUrl,
            r.CreatedAt
        )).ToList();

        return StandardResponse<List<CommunityPendingRequestDto>>.Success(dtos);
    }
}