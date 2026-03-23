namespace NYC360.API.Models.SpaceListings;

public record SpaceListingHourInput(
    DayOfWeek DayOfWeek,
    TimeOnly? OpenTime,
    TimeOnly? CloseTime,
    bool IsClosed
);
