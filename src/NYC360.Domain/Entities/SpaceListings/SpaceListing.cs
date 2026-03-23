using NYC360.Domain.Enums;
using NYC360.Domain.Enums.SpaceListings;
using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Entities.SpaceListings;

public class SpaceListing
{
    public int Id { get; set; }
    public int SubmitterUserId { get; set; }
    public UserProfile? Submitter { get; set; }

    public Category Department { get; set; }
    public SpaceListingEntityType EntityType { get; set; }
    public SpaceListingStatus Status { get; set; } = SpaceListingStatus.Pending;
    public SpaceListingOwnershipStatus OwnershipStatus { get; set; } = SpaceListingOwnershipStatus.None;

    public string Name { get; set; } = string.Empty;
    public string NameNormalized { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int? LocationId { get; set; }
    public string? LocationName { get; set; }
    public string? Borough { get; set; }
    public string? Neighborhood { get; set; }
    public string? Street { get; set; }
    public string? BuildingNumber { get; set; }
    public string? ZipCode { get; set; }

    public string? Website { get; set; }
    public string? WebsiteNormalized { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PhoneNumberNormalized { get; set; }
    public string? PublicEmail { get; set; }
    public string? PublicEmailNormalized { get; set; }
    public string? ContactName { get; set; }
    public string? SubmitterNote { get; set; }

    public bool IsClaimingOwnership { get; set; }
    public int? ClaimedByUserId { get; set; }
    public UserProfile? ClaimedByUser { get; set; }

    public List<Category> Categories { get; set; } = [];
    public List<string> Tags { get; set; } = [];

    // Business metadata (optional)
    public Domain.Enums.Users.Industry? BusinessIndustry { get; set; }
    public Domain.Enums.Users.BusinessSize? BusinessSize { get; set; }
    public Domain.Enums.Users.ServiceArea? BusinessServiceArea { get; set; }
    public Domain.Enums.Users.Services? BusinessServices { get; set; }
    public Domain.Enums.Users.OwnershipType? BusinessOwnershipType { get; set; }
    public bool? BusinessIsLicensedInNyc { get; set; }
    public bool? BusinessIsInsured { get; set; }

    // Organization metadata (optional)
    public Domain.Enums.Users.OrganizationType? OrganizationType { get; set; }
    public Domain.Enums.Users.OrganizationFundType? OrganizationFundType { get; set; }
    public List<Domain.Enums.Users.OrganizationServices> OrganizationServices { get; set; } = [];
    public bool? OrganizationIsTaxExempt { get; set; }
    public bool? OrganizationIsNysRegistered { get; set; }

    public string? SpaceItemId { get; set; }
    public string? SpaceEntityType { get; set; }
    public string? SpaceSlug { get; set; }
    public DateTime? SpacePublishedAt { get; set; }
    public DateTime? LastPublishAttemptAt { get; set; }
    public string? LastPublishError { get; set; }

    public int? AssignedReviewerUserId { get; set; }
    public UserProfile? AssignedReviewer { get; set; }
    public string? ModerationNote { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public int? ReviewedByUserId { get; set; }
    public UserProfile? ReviewedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<SpaceListingAttachment> Attachments { get; set; } = new List<SpaceListingAttachment>();
    public ICollection<SpaceListingSocialLink> SocialLinks { get; set; } = new List<SpaceListingSocialLink>();
    public ICollection<SpaceListingHour> Hours { get; set; } = new List<SpaceListingHour>();
    public ICollection<SpaceListingReviewEntry> ReviewEntries { get; set; } = new List<SpaceListingReviewEntry>();

    public void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
