namespace NYC360.API.Models.Users.ProfileUpdate;

public record UpdatePositionRequest(
    int PositionId,
    string Title,
    string Company,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsCurrent
);