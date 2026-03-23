namespace NYC360.Domain.Enums.SpaceListings;

public enum SpaceListingStatus : byte
{
    Draft = 1,
    Pending = 2,
    UnderReview = 3,
    NeedsChanges = 4,
    Approved = 5,
    Rejected = 6,
    PublishedToSpace = 7,
    Claimed = 8,
    Cancelled = 9
}
