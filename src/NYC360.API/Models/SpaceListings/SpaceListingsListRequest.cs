using NYC360.Domain.Enums;
using NYC360.Domain.Enums.SpaceListings;

namespace NYC360.API.Models.SpaceListings;

public record SpaceListingsListRequest(
    int Page = 1,
    int PageSize = 20,
    Category? Department = null,
    SpaceListingEntityType? EntityType = null,
    SpaceListingStatus? Status = null,
    string? Search = null
);
