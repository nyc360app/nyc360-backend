using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Dtos.User;

public sealed record UserEducationDto(
    string School,
    string Degree,
    string FieldOfStudy,
    DateTime StartDate,
    DateTime? EndDate
);

public static class UserEducationDtoExtensions
{
    extension(UserEducationDto)
    {
        public static UserEducationDto Map(UserEducation education) => new UserEducationDto(
            education.School,
            education.Degree,
            education.FieldOfStudy,
            education.StartDate,
            education.EndDate
        );
    }
}