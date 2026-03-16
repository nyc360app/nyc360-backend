using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.AssignLeader;

public record AssignLeaderCommand(
    int CommunityId,
    int UserId,
    int AdminUserId
) : IRequest<StandardResponse<string>>;
