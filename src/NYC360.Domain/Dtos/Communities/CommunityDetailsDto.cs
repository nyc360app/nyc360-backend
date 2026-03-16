using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Dtos.Location;

namespace NYC360.Domain.Dtos.Communities;

public record CommunityDetailsDto(
    int Id,
    string Name,
    string Slug,
    string Description,
    CommunityType? Type,
    string? AvatarUrl,
    string? CoverUrl,
    bool IsActive,
    bool IsPrivate,
    bool RequiresApproval,
    int MemberCount,
    int LeaderCount,
    int ModeratorCount,
    int? LocationId,
    LocationDto? Location,
    DateTime CreatedAt,
    DateTime LastUpdated,
    CommunityDisbandRequestDto? PendingDisbandRequest
);

public static class CommunityDetailsDtoExtensions
{
    public static CommunityDetailsDto Map(Community entity, int leaderCount, int moderatorCount, CommunityDisbandRequest? pendingRequest)
    {
        return new CommunityDetailsDto(
            entity.Id,
            entity.Name,
            entity.Slug,
            entity.Description,
            entity.Type,
            entity.AvatarUrl,
            entity.CoverUrl,
            entity.IsActive,
            entity.IsPrivate,
            entity.RequiresApproval,
            entity.MemberCount,
            leaderCount,
            moderatorCount,
            entity.LocationId,
            entity.Location != null ? LocationDto.Map(entity.Location) : null,
            entity.CreatedAt,
            entity.LastUpdated,
            pendingRequest != null ? CommunityDisbandRequestDtoExtensions.Map(pendingRequest) : null
        );
    }
}
