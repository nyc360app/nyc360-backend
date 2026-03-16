namespace NYC360.API.Models.Users.ProfileUpdate;

public record AddEducationRequest(
    string School,
    string Degree,
    string FieldOfStudy,
    DateTime StartDate,
    DateTime? EndDate
);