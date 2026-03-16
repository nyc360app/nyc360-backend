using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Commands.Disband;

public class DisbandCommunityCommandHandler(
    ICommunityRepository communityRepository,
    ICommunityPermissionService permissionService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DisbandCommunityCommand, StandardResponse<string>>
{
    public async Task<StandardResponse<string>> Handle(DisbandCommunityCommand request, CancellationToken ct)
    {
        // 1. Verify community exists
        var community = await communityRepository.GetByIdAsync(request.CommunityId, ct);
        if (community == null)
        {
            return StandardResponse<string>.Failure(
                new ApiError("community.notFound", "Community not found."));
        }

        // 2. Authorization: Only leaders can request to disband the community
        var isLeader = await permissionService.IsLeaderAsync(request.UserId, request.CommunityId, ct);
        if (!isLeader)
        {
            return StandardResponse<string>.Failure(
                new ApiError("community.unauthorized", "Only community leaders can request to disband the community."));
        }

        // 3. Check if there's already a pending disband request
        var existingRequest = await communityRepository.GetPendingDisbandRequestAsync(request.CommunityId, ct);
        if (existingRequest != null)
        {
            return StandardResponse<string>.Failure(
                new ApiError("community.disbandRequestExists", "A disband request for this community is already pending."));
        }

        // 4. Create a disband request
        var disbandRequest = new CommunityDisbandRequest
        {
            CommunityId = request.CommunityId,
            RequestedByUserId = request.UserId,
            Reason = request.Reason,
            Status = DisbandRequestStatus.Pending,
            RequestedAt = DateTime.UtcNow
        };

        await communityRepository.AddDisbandRequestAsync(disbandRequest, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<string>.Success("Disband request submitted successfully. It will be reviewed by administrators.");
    }
}
