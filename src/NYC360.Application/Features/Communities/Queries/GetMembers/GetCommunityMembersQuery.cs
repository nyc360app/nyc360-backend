using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Queries.GetMembers;

public record GetCommunityMembersQuery(
    int? UserId,
    int CommunityId,
    int Page,
    int PageSize
) : IRequest<PagedResponse<CommunityMemberDto>>;
