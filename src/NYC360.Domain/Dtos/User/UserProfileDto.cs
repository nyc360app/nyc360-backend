using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums.Users;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Enums;

namespace NYC360.Domain.Dtos.User;

public sealed record UserProfileDto(
    int Id,
    UserType Type,
    string? ImageUrl,
    string? CoverImageUrl,
    string FirstName, 
    string LastName,
    string? Headline,
    string? Bio,
    string? Email,
    string? PhoneNumber,
    AddressDto? Address,
    List<UserPositionDto> Positions,
    List<UserEducationDto> Education,
    List<CommunityMinimalDto> TopCommunities,
    List<PostDto>? RecentPosts,
    List<UserSocialLinkDto>? SocialLinks,
    UserStatsDto Stats,
    VisitorInfoDto? VisitorInfo,
    BusinessInfoDto? BusinessInfo,
    OrganizationInfoDto? OrganizationInfo,
    List<TagMinimalDto> Tags,
    List<Category> Interests
);

public static class UserProfileDtoExtensions
{
    extension(UserProfileDto)
    {
        public static UserProfileDto Map(UserProfile profile, List<PostDto>? posts = null)
        {
            return new UserProfileDto(
                profile.User!.Id,
                profile.User!.Type,
                profile.AvatarUrl,
                profile.CoverImageUrl,
                profile.FirstName!,
                profile.LastName!,
                profile.Headline,
                profile.Bio,
                profile.Email,
                profile.PhoneNumber,
                profile.Address != null ? AddressDto.Map(profile.Address!) : null,
                profile.Positions.Select(UserPositionDto.Map).ToList(),
                profile.Educations.Select(UserEducationDto.Map).ToList(),
                profile.CommunityMemberships
                    .Take(3)
                    .Select(cm => CommunityMinimalDto.Map(cm.Community!))
                    .ToList(),
                posts,
                profile.SocialLinks.Select(UserSocialLinkDto.Map).ToList(),
                UserStatsDto.Map(profile.Stats!)!,
                profile.VisitorInfo != null ? VisitorInfoDto.Map(profile.VisitorInfo) : null,
                profile.BusinessInfo != null ? BusinessInfoDto.Map(profile.BusinessInfo) : null,
                profile.OrganizationInfo != null ?  OrganizationInfoDto.Map(profile.OrganizationInfo) : null,
                profile.Tags!.Select(t => new TagMinimalDto(t.Tag!.Id, t.Tag!.Name)).ToList(),
                profile.Interests?.Select(i => i.Category).ToList() ?? []
            );
        }
    }
}