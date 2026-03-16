using NYC360.Domain.Enums;

namespace NYC360.Domain.Entities.User;

public class UserSocialLink
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public UserProfile? User { get; set; }

    public SocialPlatform Platform { get; set; }
    public string Url { get; set; } = string.Empty;
}