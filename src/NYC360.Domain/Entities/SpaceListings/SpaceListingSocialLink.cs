using NYC360.Domain.Enums;

namespace NYC360.Domain.Entities.SpaceListings;

public class SpaceListingSocialLink
{
    public int Id { get; set; }
    public int SpaceListingId { get; set; }
    public SpaceListing? SpaceListing { get; set; }
    public SocialPlatform Platform { get; set; }
    public string Url { get; set; } = string.Empty;
}
