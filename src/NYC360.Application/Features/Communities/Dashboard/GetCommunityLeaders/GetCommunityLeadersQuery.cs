using MediatR;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.GetCommunityLeaders;

public record GetCommunityLeadersQuery(
    int CommunityId,
    int Page,
    int PageSize
) : IRequest<StandardResponse<PagedResponse<CommunityMemberDto>>>;
