using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Commands.RemoveMember;

public class RemoveMemberCommandHandler(
    ICommunityRepository communityRepository,
    ICommunityPermissionService permissionService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveMemberCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(RemoveMemberCommand request, CancellationToken ct)
    {
        var community = await communityRepository.GetByIdAsync(request.CommunityId, ct);
        if (community == null) return StandardResponse.Failure(new("community.notfound", "Community not found."));

        // Rule 1: Only Owner can kick members
        if (!await permissionService.IsLeaderAsync(request.OwnerId, request.CommunityId, ct))
        {
            return StandardResponse.Failure(new("auth.unauthorized", "Only the owner can remove members."));
        }

        // Rule 2: Cannot remove yourself (the owner)
        if (request.TargetUserId == request.OwnerId)
        {
            return StandardResponse.Failure(new("community.cannot_remove_owner", "You cannot remove yourself from the community. Use the 'Leave' logic if ownership is transferred."));
        }

        var member = await communityRepository.GetMemberAsync(request.CommunityId, request.TargetUserId, ct);
        if (member == null)
        {
            return StandardResponse.Failure(new("community.member_not_found", "User is not a member of this community."));
        }

        communityRepository.RemoveMember(member);
        community.MemberCount--;
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}