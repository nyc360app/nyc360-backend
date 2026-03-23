using NYC360.Domain.Enums;

namespace NYC360.API.Models.SpaceListings;

public record SpaceListingSocialLinkInput(
    SocialPlatform Platform,
    string Url
);
