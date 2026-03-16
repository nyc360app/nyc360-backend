using MediatR;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.GetCommunityDetails;

public record GetCommunityDetailsQuery(int CommunityId) : IRequest<StandardResponse<CommunityDetailsDto>>;
