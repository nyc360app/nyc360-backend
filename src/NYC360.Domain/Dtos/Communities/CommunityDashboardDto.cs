using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Enums.Communities;

namespace NYC360.Domain.Dtos.Communities;

public record CommunityDashboardDto(
    int Id,
    string Name,
    string Slug,
    CommunityType? Type,
    int MemberCount,
    int LeaderCount,
    int ModeratorCount,
    bool IsActive,
    bool IsPrivate,
    DateTime CreatedAt,
    bool HasPendingDisbandRequest
);

public static class CommunityDashboardDtoExtensions
{
    public static CommunityDashboardDto Map(Community entity, int leaderCount, int moderatorCount, bool hasPendingDisbandRequest)
    {
        return new CommunityDashboardDto(
            entity.Id,
            entity.Name,
            entity.Slug,
            entity.Type,
            entity.MemberCount,
            leaderCount,
            moderatorCount,
            entity.IsActive,
            entity.IsPrivate,
            entity.CreatedAt,
            hasPendingDisbandRequest
        );
    }
}
