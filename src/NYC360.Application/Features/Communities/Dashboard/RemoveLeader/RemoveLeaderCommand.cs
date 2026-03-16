using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.RemoveLeader;

public record RemoveLeaderCommand(
    int CommunityId,
    int UserId,
    int AdminUserId
) : IRequest<StandardResponse<string>>;
