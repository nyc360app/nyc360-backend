using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Queries.GetPendingRequests;

public record GetPendingJoinRequestsQuery(
    int UserId,
    int CommunityId
) : IRequest<StandardResponse<List<CommunityPendingRequestDto>>>;