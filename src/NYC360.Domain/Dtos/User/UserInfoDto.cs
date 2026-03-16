using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums.Users;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Enums;

namespace NYC360.Domain.Dtos.User;

public record UserInfoDto(
    UserType Type,
    string FirstName,
    string? LastName,
    string? Headline,
    string? Bio,
    string? Email,
    string? PhoneNumber,
    string? AvatarUrl,
    string? CoverImageUrl,
    bool TwoFactorEnabled,
    bool IsPending,
    bool IsVerified,
    AddressDto? Address,
    List<Category> Interests,
    List<UserSocialLinkDto>? SocialLinks,
    List<UserPositionDto> Positions,
    List<UserEducationDto> Education,
    List<TagMinimalDto> Tags,
    BusinessInfoDto? BusinessInfo,
    VisitorInfoDto? VisitorInfo
);

public static class UserInfoDtoExtensions
{
    extension(UserInfoDto)
    {
        public static UserInfoDto Map(UserProfile user) => new(
            user.User!.Type,
            user.FirstName,
            user.LastName,
            user.Headline,
            user.Bio,
            user.Email,
            user.PhoneNumber,
            user.AvatarUrl,
            user.CoverImageUrl,
            user.User?.TwoFactorEnabled ?? false,
            user.User!.IsPending,
            user.Stats?.IsVerified ?? false,
            user.Address != null ? AddressDto.Map(user.Address) : null,
            user.Interests?.Select(i => i.Category).ToList() ?? new(),
            user.SocialLinks?.Select(UserSocialLinkDto.Map).ToList() ?? new(),
            user.Positions?.Select(UserPositionDto.Map).ToList() ?? new(),
            user.Educations?.Select(UserEducationDto.Map).ToList() ?? new(),
            user.Tags?.Where(ut => ut.Tag != null)
                .Select(ut => new TagMinimalDto(ut.TagId, ut.Tag!.Name))
                .ToList() ?? new(),
            user.BusinessInfo != null ? BusinessInfoDto.Map(user.BusinessInfo) : null,
            user.VisitorInfo != null ? VisitorInfoDto.Map(user.VisitorInfo) : null
        );
    }
}