using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;
using MediatR;
using NYC360.Application.Contracts.Services;

namespace NYC360.Application.Features.Communities.Commands.Leave;

public class LeaveCommunityCommandHandler(
    ICommunityRepository communityRepository,
    ICommunityPermissionService permissionService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<LeaveCommunityCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(LeaveCommunityCommand request, CancellationToken ct)
    {
        var community = await communityRepository.GetByIdAsync(request.CommunityId, ct);
        if (community is null)
        {
            return StandardResponse.Failure(new("community.notfound", "Community not found."));
        }
        
        if (await permissionService.IsLeaderAsync(request.UserId, request.CommunityId, ct))
        {
            return StandardResponse.Failure(
                new ApiError("community.owner_cannot_leave", "Community owner cannot leave the community.")
            );
        }
        
        var member = await communityRepository.GetMemberAsync(request.CommunityId, request.UserId, ct);
        if (member == null)
        {
            return StandardResponse.Failure(new("community.not_member", "You are not a member of this community."));
        }
        
        communityRepository.RemoveMember(member);
        community.MemberCount--;
        await unitOfWork.SaveChangesAsync(ct);
        
        return StandardResponse.Success();
    }
}