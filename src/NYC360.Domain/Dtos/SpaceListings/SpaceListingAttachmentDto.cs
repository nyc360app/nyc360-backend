using NYC360.Domain.Enums.SpaceListings;

namespace NYC360.Domain.Dtos.SpaceListings;

public record SpaceListingAttachmentDto(
    int Id,
    string Url,
    SpaceListingAttachmentType Type,
    string? FileName,
    string? ContentType,
    long? SizeBytes
);
