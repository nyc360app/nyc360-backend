using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Commands.TransferOwnership;

public class TransferOwnershipCommandHandler(
    ICommunityRepository communityRepository,
    ICommunityPermissionService permissionService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<TransferOwnershipCommand, StandardResponse<string>>
{
    public async Task<StandardResponse<string>> Handle(TransferOwnershipCommand request, CancellationToken ct)
    {
        var community = await communityRepository.GetByIdAsync(request.CommunityId, ct);
        if (community == null)
            return StandardResponse<string>.Failure(new ApiError("community.notFound", "Community not found."));

        if (request.CurrentOwnerId == request.NewOwnerId)
            return StandardResponse<string>.Failure(new ApiError("community.transfer.invalidTarget", "New owner must be different from current owner."));

        var isCurrentOwner = await permissionService.IsLeaderAsync(request.CurrentOwnerId, request.CommunityId, ct);
        if (!isCurrentOwner)
        {
            return StandardResponse<string>.Failure(
                new ApiError("community.transfer.unauthorized", "Only the current community leader can transfer ownership."));
        }

        var targetMember = await communityRepository.GetMemberAsync(request.CommunityId, request.NewOwnerId, ct);
        if (targetMember == null)
        {
            return StandardResponse<string>.Failure(
                new ApiError("community.transfer.targetNotMember", "New owner must already be a community member."));
        }

        var currentLeader = await communityRepository.GetMemberAsync(request.CommunityId, request.CurrentOwnerId, ct);
        if (currentLeader == null)
            return StandardResponse<string>.Failure(new ApiError("community.transfer.currentLeaderMissing", "Current leader membership not found."));

        currentLeader.Role = CommunityRole.Member;
        targetMember.Role = CommunityRole.Leader;

        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<string>.Success("Community ownership transferred successfully.");
    }
}
