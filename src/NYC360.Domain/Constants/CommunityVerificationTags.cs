namespace NYC360.Domain.Constants;

public static class CommunityVerificationTags
{
    public const int ApplyForCommunityLeaderBadgesId = 2000;
    public const int ApplyForCreateACommunityId = 2001;
    public const int ListCommunityOrganizationInSpaceId = 2002;

    public const string ApplyForCommunityLeaderBadgesName = "Apply for Community Leader Badges";
    public const string ApplyForCreateACommunityName = "Apply for Create a Community";
    public const string ListCommunityOrganizationInSpaceName = "List Community Organization in Space";

    public static bool TryGetName(int tagId, out string? name)
    {
        name = tagId switch
        {
            ApplyForCommunityLeaderBadgesId => ApplyForCommunityLeaderBadgesName,
            ApplyForCreateACommunityId => ApplyForCreateACommunityName,
            ListCommunityOrganizationInSpaceId => ListCommunityOrganizationInSpaceName,
            _ => null
        };

        return name is not null;
    }
}
