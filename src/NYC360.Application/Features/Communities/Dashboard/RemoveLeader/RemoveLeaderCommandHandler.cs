using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.RemoveLeader;

public class RemoveLeaderCommandHandler(
    ICommunityRepository communityRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveLeaderCommand, StandardResponse<string>>
{
    public async Task<StandardResponse<string>> Handle(RemoveLeaderCommand request, CancellationToken ct)
    {
        // 1. Verify community exists
        var community = await communityRepository.GetByIdAsync(request.CommunityId, ct);
        if (community == null)
        {
            return StandardResponse<string>.Failure(
                new ApiError("community.notFound", "Community not found."));
        }

        // 2. Verify user is a member and is a leader
        var member = await communityRepository.GetMemberAsync(request.CommunityId, request.UserId, ct);
        if (member == null)
        {
            return StandardResponse<string>.Failure(
                new ApiError("community.userNotMember", "User is not a member of this community."));
        }

        if (member.Role != CommunityRole.Leader)
        {
            return StandardResponse<string>.Failure(
                new ApiError("community.notLeader", "User is not a leader of this community."));
        }

        // 3. Check if this is the last leader
        var leaderCount = await communityRepository.GetLeaderCountAsync(request.CommunityId, ct);
        if (leaderCount <= 1)
        {
            return StandardResponse<string>.Failure(
                new ApiError("community.lastLeader", "Cannot remove the last leader. Please assign another leader first."));
        }

        // 4. Demote to member
        member.Role = CommunityRole.Member;
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<string>.Success("Leader successfully removed.");
    }
}
