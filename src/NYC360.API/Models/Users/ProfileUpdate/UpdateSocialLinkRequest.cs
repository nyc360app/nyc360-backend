using NYC360.Domain.Enums;

namespace NYC360.API.Models.Users.ProfileUpdate;

public record UpdateSocialLinkRequest(
    int LinkId, 
    SocialPlatform Platform, 
    string Url
);