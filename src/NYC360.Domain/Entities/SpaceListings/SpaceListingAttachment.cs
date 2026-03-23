using NYC360.Domain.Enums.SpaceListings;

namespace NYC360.Domain.Entities.SpaceListings;

public class SpaceListingAttachment
{
    public int Id { get; set; }
    public int SpaceListingId { get; set; }
    public SpaceListing? SpaceListing { get; set; }
    public string Url { get; set; } = string.Empty;
    public SpaceListingAttachmentType Type { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public long? SizeBytes { get; set; }
}
