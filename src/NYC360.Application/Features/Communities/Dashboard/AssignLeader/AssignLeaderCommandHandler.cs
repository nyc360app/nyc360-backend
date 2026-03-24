using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.AssignLeader;

public class AssignLeaderCommandHandler(
    ICommunityRepository communityRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AssignLeaderCommand, StandardResponse<string>>
{
    public async Task<StandardResponse<string>> Handle(AssignLeaderCommand request, CancellationToken ct)
    {
        // 1. Verify community exists
        var community = await communityRepository.GetByIdAsync(request.CommunityId, ct);
        if (community == null)
        {
            return StandardResponse<string>.Failure(
                new ApiError("community.notFound", "Community not found."));
        }

        // 2. Verify target user is a member
        var member = await communityRepository.GetMemberAsync(request.CommunityId, request.UserId, ct);
        if (member == null)
        {
            return StandardResponse<string>.Failure(
                new ApiError("community.userNotMember", "User must be a community member to be assigned as leader."));
        }

        // 3. Keep exactly one leader by demoting existing leaders first.
        var existingLeaders = await communityRepository.GetLeadersAsync(request.CommunityId, ct);
        foreach (var leader in existingLeaders)
        {
            if (leader.UserId == request.UserId)
                continue;
            leader.Role = CommunityRole.Member;
        }

        // 4. Promote target to leader (or keep as leader if already assigned).
        member.Role = CommunityRole.Leader;
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<string>.Success("Community leader assigned successfully.");
    }
}
