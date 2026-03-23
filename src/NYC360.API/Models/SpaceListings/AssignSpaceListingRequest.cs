namespace NYC360.API.Models.SpaceListings;

public record AssignSpaceListingRequest(
    int ListingId,
    int ReviewerUserId
);
