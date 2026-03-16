using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Commands.ProcessJoinRequest;

public class ProcessJoinRequestCommandHandler(
    ICommunityRepository communityRepository,
    ICommunityPermissionService permissionService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ProcessJoinRequestCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(ProcessJoinRequestCommand request, CancellationToken ct)
    {
        var community = await communityRepository.GetByIdAsync(request.CommunityId, ct);
        if (community == null) return StandardResponse.Failure(new("community.notfound", "Community not found."));

        // Rule: Only Owner can process requests
        if (!await permissionService.IsLeaderAsync(request.OwnerId, request.CommunityId, ct))
        {
            return StandardResponse.Failure(new("auth.unauthorized", "Only the owner can manage join requests."));
        }

        var joinRequest = await communityRepository.GetJoinRequestAsync(request.CommunityId, request.TargetUserId, ct);
        if (joinRequest == null)
        {
            return StandardResponse.Failure(new("community.request_not_found", "No pending request found for this user."));
        }

        if (request.Approve)
        {
            var newMember = new CommunityMember
            {
                CommunityId = request.CommunityId,
                UserId = request.TargetUserId,
                Role = CommunityRole.Member
            };
            await communityRepository.AddMemberAsync(newMember, ct);
            community.MemberCount++;
        }

        // Clean up the request regardless of Approve or Reject
        communityRepository.RemoveJoinRequest(joinRequest);
        
        await unitOfWork.SaveChangesAsync(ct);
        return StandardResponse.Success();
    }
}