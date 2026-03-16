using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Enums.Communities;

namespace NYC360.Infrastructure.Services;

public class CommunityPermissionService(ICommunityRepository communityRepository) 
    : ICommunityPermissionService
{
    public async Task<bool> IsLeaderAsync(int userId, int communityId, CancellationToken ct)
    {
        var member = await communityRepository.GetMemberAsync(communityId, userId, ct);
        return member?.Role == CommunityRole.Leader;
    }

    public async Task<bool> IsModeratorAsync(int userId, int communityId, CancellationToken ct)
    {
        var member = await communityRepository.GetMemberAsync(communityId, userId, ct);
        return member?.Role == CommunityRole.Moderator || member?.Role == CommunityRole.Leader;
    }

    public async Task<bool> IsMemberAsync(int userId, int communityId, CancellationToken ct)
    {
        return await communityRepository.IsMemberAsync(communityId, userId, ct);
    }
}