using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Queries.GetMyCommunityRequests;

public record GetMyCommunityRequestsQuery(int UserId) : IRequest<StandardResponse<MyCommunityRequestsDto>>;
