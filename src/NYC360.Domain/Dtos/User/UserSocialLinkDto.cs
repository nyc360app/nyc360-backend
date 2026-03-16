using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums;

namespace NYC360.Domain.Dtos.User;

public record UserSocialLinkDto(int Id, SocialPlatform Platform, string Url);

public static class UserSocialLinkDtoExtensions
{
    extension(UserSocialLinkDto)
    {
        public static UserSocialLinkDto Map(UserSocialLink link)
        {
            return new UserSocialLinkDto(link.Id, link.Platform, link.Url);
        }
    }
}