using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Queries.SearchMembers;

public record SearchCommunityMembersQuery(
    int? UserId,
    int CommunityId,
    string? SearchTerm,
    int Page,
    int PageSize
) : IRequest<PagedResponse<CommunityMemberDto>>;
