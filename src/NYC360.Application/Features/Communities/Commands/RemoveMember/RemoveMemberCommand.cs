using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Commands.RemoveMember;

public record RemoveMemberCommand(
    int OwnerId,
    int CommunityId,
    int TargetUserId
) : IRequest<StandardResponse>;