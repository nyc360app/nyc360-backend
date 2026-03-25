using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Enums.Communities;

namespace NYC360.Domain.Dtos.Communities;

public record CommunityDto(
    int Id,
    string Name,
    string Slug,
    string Description,
    string? Rules,
    CommunityType? Type,
    string? ImageUrl,
    string? CoverUrl,
    bool IsPrivate,
    bool RequiresApproval,
    bool AnyoneCanPost,
    bool IsFeatured,
    int MemberCount,
    int? LocationId,
    LocationDto? Location,
    CommunityLeaderBasicDto? Leader,
    bool IsVerifiedLeader,
    DateTime CreatedAt,
    string Status,
    CommunityContentSummaryDto? ContentSummary
);

public record CommunityLeaderBasicDto(
    int UserId,
    string FullName,
    string? AvatarUrl,
    string? UserName
);

public static class CommunityDtoExtensions
{
    extension(CommunityDto)
    {
        public static CommunityDto Map(
            Community entity,
            CommunityLeaderBasicDto? leader = null,
            bool isVerifiedLeader = false,
            CommunityContentSummaryDto? contentSummary = null)
        {
            return new CommunityDto(
                entity.Id,
                entity.Name,
                entity.Slug,
                entity.Description,
                entity.Rules,
                entity.Type,
                entity.AvatarUrl,
                entity.CoverUrl,
                entity.IsPrivate,
                entity.RequiresApproval,
                entity.AnyoneCanPost,
                entity.IsFeatured,
                entity.MemberCount,
                entity.LocationId,
                entity.Location is null ? null : LocationDto.Map(entity.Location),
                leader,
                isVerifiedLeader,
                entity.CreatedAt,
                entity.IsActive ? "active" : "inactive",
                contentSummary
            );
        }
    }
}
