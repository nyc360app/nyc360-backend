using NYC360.Domain.Entities.Professions;
using NYC360.Domain.Enums.Professions;
using NYC360.Domain.Dtos.User;

namespace NYC360.Domain.Dtos.Professions;

public record JobApplicationDetailsDto(
    int ApplicationId,
    ApplicationStatus Status,
    DateTime AppliedAt,
    string? CoverLetter,
    string? ResumeUrl,
    int ApplicantId, 
    string ApplicantUsername, 
    string ApplicantFullName, 
    string? ApplicantAvatarUrl,
    string? ApplicantBio,
    List<UserEducationDto> ApplicantEducations,
    List<UserPositionDto> ApplicantExperiences
);

public static class JobApplicationDetailsDtoExtensions
{
    extension(JobApplicationDetailsDto)
    {
        public static JobApplicationDetailsDto Map(JobApplication application) => new JobApplicationDetailsDto(
            application.Id,
            application.Status,
            application.AppliedAt,
            application.CoverLetter,
            application.ResumeUrl,
            application.Applicant.UserId,
            application.Applicant.User!.UserName!,
            application.Applicant.User.GetFullName(),
            application.Applicant.AvatarUrl,
            application.Applicant.Bio,
            application.Applicant.Educations.Select(UserEducationDto.Map).ToList(),
            application.Applicant.Positions.Select(UserPositionDto.Map).ToList()
        );
    }
}