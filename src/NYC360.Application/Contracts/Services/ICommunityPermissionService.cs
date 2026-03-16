namespace NYC360.Application.Contracts.Services;

public interface ICommunityPermissionService
{
    Task<bool> IsLeaderAsync(int userId, int communityId, CancellationToken ct);
    Task<bool> IsModeratorAsync(int userId, int communityId, CancellationToken ct);
    Task<bool> IsMemberAsync(int userId, int communityId, CancellationToken ct);
}