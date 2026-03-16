using NYC360.Domain.Entities.Professions;
using NYC360.Domain.Enums.Professions;

namespace NYC360.Domain.Dtos.Professions;

public record JobApplicationDto(
    int ApplicationId,
    JobOfferMinimalDto Offer,
    ApplicationStatus Status,
    DateTime AppliedAt
);

public static class JobApplicationDtoExtensions
{
    extension(JobApplicationDto)
    {
        public static JobApplicationDto Map(JobApplication application)
        {
            return new JobApplicationDto(
                application.Id,
                JobOfferMinimalDto.Map(application.JobOffer),
                application.Status,
                application.AppliedAt
            );
        }
    }
}