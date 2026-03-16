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

        // 3. Check if user is already a leader
        if (member.Role == CommunityRole.Leader)
        {
            return StandardResponse<string>.Failure(
                new ApiError("community.alreadyLeader", "User is already a leader of this community."));
        }

        // 4. Promote to leader
        member.Role = CommunityRole.Leader;
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<string>.Success("User successfully assigned as community leader.");
    }
}
