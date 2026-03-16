using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Queries.GetMembers;

public class GetCommunityMembersQueryHandler(ICommunityRepository communityRepository)
    : IRequestHandler<GetCommunityMembersQuery, PagedResponse<CommunityMemberDto>>
{
    public async Task<PagedResponse<CommunityMemberDto>> Handle(GetCommunityMembersQuery request, CancellationToken ct)
    {
        var members = await communityRepository.GetMembersPaginatedAsync(
            request.CommunityId, 
            request.Page, 
            request.PageSize, 
            ct);

        var totalCount = await communityRepository.GetMemberCountAsync(request.CommunityId, ct);

        var dtos = members.Select(m => new CommunityMemberDto(
            m.UserId,
            m.User?.GetFullName() ?? "Unknown",
            m.User!.AvatarUrl,
            m.Role.ToString(),
            m.JoinedAt
        ));

        return PagedResponse<CommunityMemberDto>.Create(dtos, request.Page, request.PageSize, totalCount);
    }
}