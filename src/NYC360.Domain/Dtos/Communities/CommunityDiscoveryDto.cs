using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Enums.Communities;

namespace NYC360.Domain.Dtos.Communities;

public record CommunityDiscoveryDto(
    int Id,
    string Name,
    string Slug,
    string Description,
    string? AvatarUrl,
    CommunityType? Type,
    int MemberCount,
    bool IsPrivate
);

public static class CommunityDiscoveryDtoExtensions
{
    extension(CommunityDiscoveryDto)
    {
        public static CommunityDiscoveryDto Map(Community community)
        {
            return new CommunityDiscoveryDto(
                community.Id,
                community.Name,
                community.Slug,
                community.Description,
                community.AvatarUrl,
                community.Type,
                community.MemberCount,
                community.IsPrivate
            );
        }
    }
}