using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Constants;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Users;

namespace NYC360.Infrastructure.Services;

public class NewsAuthorizationService(IUserRepository userRepository) : INewsAuthorizationService
{
    public async Task<NewsAccessDto?> GetAccessAsync(int userId, CancellationToken ct)
    {
        var profile = await userRepository.GetProfileInfoByUserIdAsync(userId, ct);
        if (profile?.User == null)
            return null;

        var roles = await userRepository.GetUserRolesAsync(profile.User, ct);
        var isStaff = roles.Contains("SuperAdmin", StringComparer.OrdinalIgnoreCase)
            || roles.Contains("Admin", StringComparer.OrdinalIgnoreCase)
            || profile.User.Type == UserType.Admin;

        var grantedNewsTags = profile.Tags?
            .Where(ut => ut.Tag?.Division == Category.News)
            .Select(ut => ut.Tag!)
            .DistinctBy(tag => tag.Id)
            .ToList() ?? [];

        var tagNames = grantedNewsTags
            .Select(tag => tag.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var canSubmitContent = isStaff || tagNames.Overlaps(NewsDepartmentTags.ContentCreatorNames);
        var canModerateContent = isStaff || tagNames.Overlaps(NewsDepartmentTags.ModerationNames);
        var canPublishContent = isStaff || tagNames.Overlaps(NewsDepartmentTags.DirectPublishingNames);
        var canConnectRss = canPublishContent;
        var canReviewRssRequests = canModerateContent;
        var canListNewsOrganizationInSpace = isStaff
            || (profile.User.Type == UserType.Organization && tagNames.Contains(NewsDepartmentTags.ListNewsOrganizationInSpaceName));
        var grantedKeys = BuildGrantedKeys(isStaff, canSubmitContent, canListNewsOrganizationInSpace, tagNames);

        return new NewsAccessDto(
            isStaff,
            profile.User.Type == UserType.Organization,
            canSubmitContent,
            canModerateContent,
            canPublishContent,
            canConnectRss,
            canReviewRssRequests,
            canListNewsOrganizationInSpace,
            grantedKeys,
            grantedNewsTags.Select(tag => tag.Map()).ToList()
        );
    }

    private static List<string> BuildGrantedKeys(
        bool isStaff,
        bool canSubmitContent,
        bool canListNewsOrganizationInSpace,
        HashSet<string> tagNames)
    {
        var keys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (isStaff || tagNames.Contains(NewsDepartmentTags.PublisherBadgeName))
            keys.Add(NewsDepartmentKeys.PublisherKey);

        if (isStaff || tagNames.Contains(NewsDepartmentTags.AssistantPublisherBadgeName))
            keys.Add(NewsDepartmentKeys.AssistantPublisherKey);

        if (isStaff
            || tagNames.Contains(NewsDepartmentTags.JournalistBadgeName)
            || tagNames.Contains(NewsDepartmentTags.TraineeJournalistBadgeName)
            || tagNames.Contains(NewsDepartmentTags.PublisherBadgeName)
            || tagNames.Contains(NewsDepartmentTags.AssistantPublisherBadgeName))
        {
            keys.Add(NewsDepartmentKeys.JournalistKey);
        }

        if (isStaff
            || tagNames.Contains(NewsDepartmentTags.ContributorBadgeName)
            || tagNames.Contains(NewsDepartmentTags.PublisherBadgeName)
            || tagNames.Contains(NewsDepartmentTags.AssistantPublisherBadgeName))
        {
            keys.Add(NewsDepartmentKeys.ContentContributorKey);
        }

        if (isStaff
            || tagNames.Contains(NewsDepartmentTags.DocumentorBadgeName)
            || tagNames.Contains(NewsDepartmentTags.PublisherBadgeName)
            || tagNames.Contains(NewsDepartmentTags.AssistantPublisherBadgeName))
        {
            keys.Add(NewsDepartmentKeys.DocumentorKey);
        }

        if (canSubmitContent)
            keys.Add(NewsDepartmentKeys.LocationListingKey);

        if (canListNewsOrganizationInSpace)
            keys.Add(NewsDepartmentKeys.NewsOrganizationLocationListingKey);

        return keys.ToList();
    }
}
