using NYC360.Domain.Enums.SpaceListings;

namespace NYC360.API.Models.SpaceListings;

public record ReviewSpaceListingRequest(
    int ListingId,
    SpaceListingStatus Decision,
    string? ModerationNote
);
