using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Enums.Communities;

namespace NYC360.Domain.Dtos.Communities;

public record CommunityDto(
    int Id,
    string Name,
    string Slug,
    string Description,
    CommunityType? Type,
    string? ImageUrl,
    string? CoverUrl,
    bool IsPrivate,
    int MemberCount
);

public static class CommunityDtoExtensions
{
    extension(CommunityDto)
    {
        public static CommunityDto Map(Community entity)
        {
            return new CommunityDto(
                entity.Id,
                entity.Name,
                entity.Slug,
                entity.Description,
                entity.Type,
                entity.AvatarUrl,
                entity.CoverUrl,
                entity.IsPrivate,
                entity.MemberCount
            );
        }
    }
}