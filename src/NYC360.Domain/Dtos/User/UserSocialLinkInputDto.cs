using NYC360.Domain.Enums;

namespace NYC360.Domain.Dtos.User;

public record UserSocialLinkInputDto(SocialPlatform Platform, string Url);