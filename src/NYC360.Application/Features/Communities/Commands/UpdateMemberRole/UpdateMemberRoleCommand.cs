using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Commands.UpdateMemberRole;

public record UpdateMemberRoleCommand(
    int UserId,
    int CommunityId,
    int TargetUserId,
    CommunityRole NewRole
) : IRequest<StandardResponse>;
