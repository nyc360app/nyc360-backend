using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.GetAllCommunities;

public class GetAllCommunitiesQueryHandler(ICommunityRepository communityRepository)
    : IRequestHandler<GetAllCommunitiesQuery, StandardResponse<PagedResponse<CommunityDashboardDto>>>
{
    public async Task<StandardResponse<PagedResponse<CommunityDashboardDto>>> Handle(GetAllCommunitiesQuery request, CancellationToken ct)
    {
        var (communities, total) = await communityRepository.GetAllCommunitiesPaginatedAsync(
            request.SearchTerm,
            request.Type,
            request.LocationId,
            request.HasDisbandRequest,
            request.Page,
            request.PageSize,
            ct);

        var dtos = communities.Select(c =>
        {
            var leaderCount = c.Members.Count(m => m.Role == CommunityRole.Leader);
            var moderatorCount = c.Members.Count(m => m.Role == CommunityRole.Moderator);
            var hasPendingRequest = c.DisbandRequests.Any(r => r.Status == DisbandRequestStatus.Pending);

            return CommunityDashboardDtoExtensions.Map(c, leaderCount, moderatorCount, hasPendingRequest);
        }).ToList();

        var pagedResponse = PagedResponse<CommunityDashboardDto>.Create(dtos, request.Page, request.PageSize, total);

        return StandardResponse<PagedResponse<CommunityDashboardDto>>.Success(pagedResponse);
    }
}
