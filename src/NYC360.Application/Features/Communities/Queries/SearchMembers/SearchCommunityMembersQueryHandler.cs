using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Queries.SearchMembers;

public class SearchCommunityMembersQueryHandler(
    ICommunityRepository communityRepository,
    ICommunityPermissionService permissionService)
    : IRequestHandler<SearchCommunityMembersQuery, PagedResponse<CommunityMemberDto>>
{
    public async Task<PagedResponse<CommunityMemberDto>> Handle(SearchCommunityMembersQuery request, CancellationToken ct)
    {
        // 1. Fetch community metadata to check privacy
        var community = await communityRepository.GetByIdAsync(request.CommunityId, ct);
        if (community == null)
        {
            return PagedResponse<CommunityMemberDto>.Create(
                Enumerable.Empty<CommunityMemberDto>(), 
                request.Page, 
                request.PageSize, 
                0);
        }

        // 2. Privacy Check: If private, user must be a member
        if (community.IsPrivate)
        {
            if (request.UserId == null)
            {
                return PagedResponse<CommunityMemberDto>.Create(
                    Enumerable.Empty<CommunityMemberDto>(), 
                    request.Page, 
                    request.PageSize, 
                    0);
            }

            var isMember = await permissionService.IsMemberAsync(request.UserId.Value, request.CommunityId, ct);
            if (!isMember)
            {
                return PagedResponse<CommunityMemberDto>.Create(
                    Enumerable.Empty<CommunityMemberDto>(), 
                    request.Page, 
                    request.PageSize, 
                    0);
            }
        }

        // 3. Search members
        var (members, totalCount) = await communityRepository.SearchMembersAsync(
            request.CommunityId,
            request.SearchTerm ?? string.Empty,
            request.Page,
            request.PageSize,
            ct);

        // 3. Map to DTOs
        var dtos = members.Select(m => new CommunityMemberDto(
            m.UserId,
            m.User?.GetFullName() ?? "Unknown",
            m.User?.AvatarUrl,
            m.Role.ToString(),
            m.JoinedAt
        ));

        return PagedResponse<CommunityMemberDto>.Create(dtos, request.Page, request.PageSize, totalCount);
    }
}
