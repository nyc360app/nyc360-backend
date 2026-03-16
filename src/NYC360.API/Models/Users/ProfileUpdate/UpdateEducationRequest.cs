namespace NYC360.API.Models.Users.ProfileUpdate;

public record UpdateEducationRequest(
    int EducationId,
    string School,
    string Degree,
    string FieldOfStudy,
    DateTime StartDate,
    DateTime? EndDate
);