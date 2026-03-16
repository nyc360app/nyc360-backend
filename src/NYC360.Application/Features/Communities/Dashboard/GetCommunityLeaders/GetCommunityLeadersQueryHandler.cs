using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.GetCommunityLeaders;

public class GetCommunityLeadersQueryHandler(ICommunityRepository communityRepository)
    : IRequestHandler<GetCommunityLeadersQuery, StandardResponse<PagedResponse<CommunityMemberDto>>>
{
    public async Task<StandardResponse<PagedResponse<CommunityMemberDto>>> Handle(GetCommunityLeadersQuery request, CancellationToken ct)
    {
        var community = await communityRepository.GetByIdAsync(request.CommunityId, ct);
        if (community == null)
        {
            return StandardResponse<PagedResponse<CommunityMemberDto>>.Failure(new ApiError("community.notFound", "Community not found"));
        }

        var (leaders, total) = (await communityRepository.GetLeadersAsync(request.CommunityId, ct), await communityRepository.GetLeaderCountAsync(request.CommunityId, ct));
        
        // Note: GetLeadersAsync returns List, so we manually page if the repo method doesn't support paging (Wait, repo implementation returned List but query here takes Page/PageSize)
        // Let's check ICommunityRepository.GetLeadersAsync signature.
        
        // ICommunityRepository.cs: Task<List<CommunityMember>> GetLeadersAsync(int communityId, CancellationToken ct);
        // It returns ALL leaders. We should probably page it in memory or update repo. 
        // Given leader count is usually small, in-memory paging is fine for now but let's be safe.
        
        var pagedLeaders = leaders
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(CommunityMemberDto.Map)
            .ToList();

        var pagedResponse = PagedResponse<CommunityMemberDto>.Create(pagedLeaders, request.Page, request.PageSize, total);

        return StandardResponse<PagedResponse<CommunityMemberDto>>.Success(pagedResponse);
    }
}
