namespace NYC360.Domain.Dtos.Communities;

public record MyCommunityJoinRequestDto(
    int CommunityId,
    string CommunityName,
    string CommunitySlug,
    string? CommunityAvatar,
    DateTime RequestedAt
);

public static class MyCommunityJoinRequestDtoExtensions
{
    extension(MyCommunityJoinRequestDto)
    {
        public static MyCommunityJoinRequestDto Map(NYC360.Domain.Entities.Communities.CommunityJoinRequest entity)
        {
            return new MyCommunityJoinRequestDto(
                entity.CommunityId,
                entity.Community?.Name ?? "Unknown",
                entity.Community?.Slug ?? string.Empty,
                entity.Community?.AvatarUrl,
                entity.CreatedAt
            );
        }
    }
}
