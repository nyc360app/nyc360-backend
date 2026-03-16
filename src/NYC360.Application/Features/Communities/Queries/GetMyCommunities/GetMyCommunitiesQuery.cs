using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;
using MediatR;
using NYC360.Domain.Enums.Communities;

namespace NYC360.Application.Features.Communities.Queries.GetMyCommunities;

public record GetMyCommunitiesQuery(
    int UserId,
    string? SearchTerm,
    CommunityType? Type,
    int? LocationId,
    int Page,
    int PageSize
) : IRequest<PagedResponse<CommunityDiscoveryDto>>;