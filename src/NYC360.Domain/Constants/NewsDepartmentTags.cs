namespace NYC360.Domain.Constants;

public static class NewsDepartmentTags
{
    public const int PublisherBadgeId = 3000;
    public const int AssistantPublisherBadgeId = 3001;
    public const int JournalistBadgeId = 3002;
    public const int AuthorBadgeId = 3003;
    public const int DocumentorBadgeId = 3004;
    public const int ContributorBadgeId = 3005;
    public const int TraineeJournalistBadgeId = 3006;
    public const int ListNewsOrganizationInSpaceId = 3007;
    public const int VerifiedPublisherId = 3008;
    public const int ProbationaryPublisherId = 3009;

    public const string PublisherBadgeName = "D08.0 Publisher Badge";
    public const string AssistantPublisherBadgeName = "D08.0.1 Assistant Publisher Badge";
    public const string JournalistBadgeName = "D08.1 Journalist Badge";
    public const string AuthorBadgeName = "D08.2 Author Badge";
    public const string DocumentorBadgeName = "D08.3 Documentor Badge";
    public const string ContributorBadgeName = "D08.4 Contributor Badge";
    public const string TraineeJournalistBadgeName = "D08.5 Trainee Journalist Badge";
    public const string ListNewsOrganizationInSpaceName = "D08.6 List News Organization in THE360.SPACE";
    public const string VerifiedPublisherName = "Verified Publisher";
    public const string ProbationaryPublisherName = "Probationary Publisher";

    public static IReadOnlyList<int> ContentCreatorIds =>
    [
        PublisherBadgeId,
        AssistantPublisherBadgeId,
        JournalistBadgeId,
        AuthorBadgeId,
        DocumentorBadgeId,
        ContributorBadgeId,
        TraineeJournalistBadgeId
    ];

    public static IReadOnlySet<string> ContentCreatorNames => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        PublisherBadgeName,
        AssistantPublisherBadgeName,
        JournalistBadgeName,
        AuthorBadgeName,
        DocumentorBadgeName,
        ContributorBadgeName,
        TraineeJournalistBadgeName
    };

    public static IReadOnlySet<string> ModerationNames => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        PublisherBadgeName,
        AssistantPublisherBadgeName,
        ProbationaryPublisherName
    };

    public static IReadOnlySet<string> DirectPublishingNames => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        PublisherBadgeName
    };

    public static bool TryGetName(int tagId, out string? name)
    {
        name = tagId switch
        {
            PublisherBadgeId => PublisherBadgeName,
            AssistantPublisherBadgeId => AssistantPublisherBadgeName,
            JournalistBadgeId => JournalistBadgeName,
            AuthorBadgeId => AuthorBadgeName,
            DocumentorBadgeId => DocumentorBadgeName,
            ContributorBadgeId => ContributorBadgeName,
            TraineeJournalistBadgeId => TraineeJournalistBadgeName,
            ListNewsOrganizationInSpaceId => ListNewsOrganizationInSpaceName,
            VerifiedPublisherId => VerifiedPublisherName,
            ProbationaryPublisherId => ProbationaryPublisherName,
            _ => null
        };

        return name is not null;
    }
}
