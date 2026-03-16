namespace NYC360.API.Models.Housing;

public record PublishHouseListingRequest(
    int HouseId,
    bool IsPublished
);
