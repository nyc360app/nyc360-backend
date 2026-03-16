using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Commands.ProcessJoinRequest;

public record ProcessJoinRequestCommand(
    int OwnerId,
    int CommunityId,
    int TargetUserId,
    bool Approve
) : IRequest<StandardResponse>;