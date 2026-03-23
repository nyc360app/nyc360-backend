using NYC360.Domain.Enums.SpaceListings;
using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Entities.SpaceListings;

public class SpaceListingReviewEntry
{
    public int Id { get; set; }
    public int SpaceListingId { get; set; }
    public SpaceListing? SpaceListing { get; set; }
    public int ReviewerUserId { get; set; }
    public UserProfile? Reviewer { get; set; }
    public SpaceListingStatus FromStatus { get; set; }
    public SpaceListingStatus ToStatus { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
