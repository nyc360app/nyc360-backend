using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Constants;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Tags;
using NYC360.Domain.Enums.Users;
using NYC360.Domain.Entities.Tags;

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
        var grantedNewsTagDtos = BuildGrantedNewsTags(
            grantedNewsTags,
            isStaff,
            canListNewsOrganizationInSpace);

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
            grantedNewsTagDtos
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

    private static List<TagDto> BuildGrantedNewsTags(
        List<Tag> actualGrantedNewsTags,
        bool isStaff,
        bool canListNewsOrganizationInSpace)
    {
        var effectiveTags = actualGrantedNewsTags
            .Select(tag => tag.Map())
            .ToDictionary(tag => tag.Id);

        if (!isStaff)
            return effectiveTags.Values.ToList();

        // Staff bypasses News badge checks across the feature set. Surface the effective
        // operational badges as well so clients that still gate by badge presence stay aligned
        // with the backend authorization result.
        AddSyntheticTag(effectiveTags, NewsDepartmentTags.PublisherBadgeId, NewsDepartmentTags.PublisherBadgeName);
        AddSyntheticTag(effectiveTags, NewsDepartmentTags.AssistantPublisherBadgeId, NewsDepartmentTags.AssistantPublisherBadgeName);
        AddSyntheticTag(effectiveTags, NewsDepartmentTags.JournalistBadgeId, NewsDepartmentTags.JournalistBadgeName);
        AddSyntheticTag(effectiveTags, NewsDepartmentTags.AuthorBadgeId, NewsDepartmentTags.AuthorBadgeName);
        AddSyntheticTag(effectiveTags, NewsDepartmentTags.DocumentorBadgeId, NewsDepartmentTags.DocumentorBadgeName);
        AddSyntheticTag(effectiveTags, NewsDepartmentTags.ContributorBadgeId, NewsDepartmentTags.ContributorBadgeName);
        AddSyntheticTag(effectiveTags, NewsDepartmentTags.TraineeJournalistBadgeId, NewsDepartmentTags.TraineeJournalistBadgeName);

        if (canListNewsOrganizationInSpace)
        {
            AddSyntheticTag(
                effectiveTags,
                NewsDepartmentTags.ListNewsOrganizationInSpaceId,
                NewsDepartmentTags.ListNewsOrganizationInSpaceName);
        }

        return effectiveTags.Values.OrderBy(tag => tag.Id).ToList();
    }

    private static void AddSyntheticTag(Dictionary<int, TagDto> effectiveTags, int id, string name)
    {
        if (effectiveTags.ContainsKey(id))
            return;

        effectiveTags[id] = new TagDto(id, name, TagType.Professional, Category.News, null, null);
    }
}
