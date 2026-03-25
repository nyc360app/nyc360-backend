using MediatR;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.GetLeaderApplicationDetails;

public record GetCommunityLeaderApplicationDetailsQuery(int ApplicationId)
    : IRequest<StandardResponse<CommunityLeaderApplicationAdminDetailsDto>>;
