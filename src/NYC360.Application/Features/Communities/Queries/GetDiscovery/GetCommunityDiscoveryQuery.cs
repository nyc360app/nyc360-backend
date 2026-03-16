using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Queries.GetDiscovery;

public record GetCommunityDiscoveryQuery(
    int UserId,
    string? SearchTerm,
    CommunityType? Type,
    int? LocationId,
    int Page,
    int PageSize
) : IRequest<PagedResponse<CommunityDiscoveryDto>>;