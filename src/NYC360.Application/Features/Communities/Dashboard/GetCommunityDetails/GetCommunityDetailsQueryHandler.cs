using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.GetCommunityDetails;

public class GetCommunityDetailsQueryHandler(ICommunityRepository communityRepository)
    : IRequestHandler<GetCommunityDetailsQuery, StandardResponse<CommunityDetailsDto>>
{
    public async Task<StandardResponse<CommunityDetailsDto>> Handle(GetCommunityDetailsQuery request, CancellationToken ct)
    {
        var community = await communityRepository.GetByIdAsync(request.CommunityId, ct);
        if (community == null)
        {
            return StandardResponse<CommunityDetailsDto>.Failure(new ApiError("community.notFound", "Community not found"));
        }

        var leaderCount = await communityRepository.GetLeaderCountAsync(request.CommunityId, ct);
        var moderatorCount = await communityRepository.GetModeratorCountAsync(request.CommunityId, ct);
        var pendingRequest = await communityRepository.GetPendingDisbandRequestAsync(request.CommunityId, ct);

        var dto = CommunityDetailsDtoExtensions.Map(community, leaderCount, moderatorCount, pendingRequest);

        return StandardResponse<CommunityDetailsDto>.Success(dto);
    }
}
