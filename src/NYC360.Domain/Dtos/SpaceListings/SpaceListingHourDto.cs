namespace NYC360.Domain.Dtos.SpaceListings;

public record SpaceListingHourDto(
    int Id,
    DayOfWeek DayOfWeek,
    TimeOnly? OpenTime,
    TimeOnly? CloseTime,
    bool IsClosed
);
