namespace NYC360.API.Models.Users.ProfileUpdate;

public record AddPositionRequest(
    string Title,
    string Company,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsCurrent
);