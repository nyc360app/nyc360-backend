using NYC360.Domain.Enums;
using NYC360.Domain.Enums.SpaceListings;

namespace NYC360.Domain.Dtos.SpaceListings;

public record SpaceListingListItemDto(
    int Id,
    string Name,
    Category Department,
    SpaceListingEntityType EntityType,
    SpaceListingStatus Status,
    SpaceListingOwnershipStatus OwnershipStatus,
    string? SpaceItemId,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
