using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Commands.UpdateMemberRole;

public class UpdateMemberRoleCommandHandler(
    ICommunityRepository communityRepository,
    ICommunityPermissionService permissionService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateMemberRoleCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateMemberRoleCommand request, CancellationToken ct)
    {
        // 1. Authorization: Only the owner can update roles
        var isOwner = await permissionService.IsLeaderAsync(request.UserId, request.CommunityId, ct);
        if (!isOwner)
        {
            return StandardResponse.Failure(
                new ApiError("community.unauthorized", "Only the community owner can update member roles."));
        }

        // 2. Fetch the target member
        var targetMember = await communityRepository.GetMemberAsync(request.CommunityId, request.TargetUserId, ct);
        if (targetMember == null)
        {
            return StandardResponse.Failure(
                new ApiError("community.memberNotFound", "Target member not found in this community."));
        }

        // 3. Logic: Cannot update role if target is the owner (must use TransferOwnership)
        if (targetMember.Role == CommunityRole.Leader)
        {
            return StandardResponse.Failure(
                new ApiError("community.cannotModifyOwner", "To change the owner's role, you must transfer ownership of the community."));
        }

        // 4. Logic: Cannot promote someone to Owner via this endpoint
        if (request.NewRole == CommunityRole.Leader)
        {
            return StandardResponse.Failure(
                new ApiError("community.useTransferOwnership", "To promote a member to owner, please use the transfer ownership feature."));
        }

        // 5. Update Role
        targetMember.Role = request.NewRole;
        
        // 6. Persist
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}
