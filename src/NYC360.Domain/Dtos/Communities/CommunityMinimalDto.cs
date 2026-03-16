using NYC360.Domain.Entities.Communities;

namespace NYC360.Domain.Dtos.Communities;

public record CommunityMinimalDto(
    int Id,
    string Name, 
    string Slug
);

public static class CommunityMinimalDtoExtension
{
    extension(CommunityMinimalDto)
    {
        public static CommunityMinimalDto Map(Community community) =>
            new(
                community.Id, 
                community.Name, 
                community.Slug
            );
    }
}