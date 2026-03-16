using NYC360.Domain.Enums;

namespace NYC360.API.Models.Users.ProfileUpdate;

public record AddSocialLinkRequest(
    SocialPlatform Platform, 
    string Url
);