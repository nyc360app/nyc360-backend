using MediatR;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.GetAllCommunities;

public record GetAllCommunitiesQuery(
    int Page,
    int PageSize,
    string? SearchTerm,
    CommunityType? Type,
    int? LocationId,
    bool? HasDisbandRequest
) : IRequest<StandardResponse<PagedResponse<CommunityDashboardDto>>>;
