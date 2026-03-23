using NYC360.Domain.Enums;

namespace NYC360.Domain.Dtos.SpaceListings;

public record SpaceListingSocialLinkDto(
    int Id,
    SocialPlatform Platform,
    string Url
);
