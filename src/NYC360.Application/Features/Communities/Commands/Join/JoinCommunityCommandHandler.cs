using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Commands.Join;

public class JoinCommunityCommandHandler(
    ICommunityRepository communityRepository,
    ICommunityPermissionService permissionService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<JoinCommunityCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(JoinCommunityCommand request, CancellationToken ct)
    {
        var community = await communityRepository.GetByIdAsync(request.CommunityId, ct);
        if (community is null)
        {
            return StandardResponse.Failure(new("community.notfound", "Community not found."));
        }

        // Guard: Owner is already a member by default
        if (await permissionService.IsLeaderAsync(request.UserId, request.CommunityId, ct))
        {
            return StandardResponse.Failure(new("community.owner_cannot_join", "Owner is already part of the community."));
        }

        // Guard: Prevent double membership
        var alreadyMember = await communityRepository.IsMemberAsync(request.CommunityId, request.UserId, ct);
        if (alreadyMember)
        {
            return StandardResponse.Failure(new("community.already_member", "You are already a member of this community."));
        }

        // Logic: Private communities (Invite only)
        if (community.IsPrivate)
        {
            return StandardResponse.Failure(new("community.private", "This community is private and cannot be joined."));
        }

        // Logic: Requires Approval flow
        if (community.RequiresApproval)
        {
            var existingRequest = await communityRepository.GetJoinRequestAsync(request.CommunityId, request.UserId, ct);
            if (existingRequest != null)
            {
                return StandardResponse.Failure(new("community.request_pending", "Your join request is already pending."));
            }

            var joinRequest = new CommunityJoinRequest
            {
                CommunityId = community.Id,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await communityRepository.AddJoinRequestAsync(joinRequest, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return StandardResponse.Success(); // For now, we return success as the "Request Sent" state
        }

        // Logic: Standard Direct Join
        var member = new CommunityMember 
        { 
            CommunityId = community.Id, 
            UserId = request.UserId, 
            Role = CommunityRole.Member 
        };
        
        await communityRepository.AddMemberAsync(member, ct);
        community.MemberCount++;
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}