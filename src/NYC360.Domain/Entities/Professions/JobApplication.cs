using NYC360.Domain.Enums.Professions;
using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Entities.Professions;

public class JobApplication
{
    public int Id { get; set; }
    public int JobOfferId { get; set; }
    public JobOffer JobOffer { get; set; } = null!;
    
    public int ApplicantId { get; set; }
    public UserProfile Applicant { get; set; } = null!;

    // Application Metadata
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    public string? CoverLetter { get; set; }
    public string? ResumeUrl { get; set; }
    
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StatusChangedAt { get; set; }
}