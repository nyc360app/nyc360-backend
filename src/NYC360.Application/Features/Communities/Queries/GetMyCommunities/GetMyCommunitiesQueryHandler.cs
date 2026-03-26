using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Queries.GetMyCommunities;

public class GetMyCommunitiesQueryHandler(ICommunityRepository communityRepository)
    : IRequestHandler<GetMyCommunitiesQuery, PagedResponse<CommunityDiscoveryDto>>
{
    public async Task<PagedResponse<CommunityDiscoveryDto>> Handle(GetMyCommunitiesQuery request, CancellationToken ct)
    {
        // We'll need a specialized method in the repository for filtered searching
        var (items, total) = await communityRepository.SearchUserCommunitiesAsync(
            request.UserId,
            request.SearchTerm,
            request.Type,
            request.LocationId,
            request.Page,
            request.PageSize,
            ct);

        var dtos = items.Select(c => CommunityDiscoveryDto.Map(c));

        return PagedResponse<CommunityDiscoveryDto>.Create(dtos, request.Page, request.PageSize, total);
    }
}
