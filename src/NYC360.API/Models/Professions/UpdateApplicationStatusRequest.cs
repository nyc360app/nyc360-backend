using NYC360.Domain.Enums.Professions;

namespace NYC360.API.Models.Professions;

public record UpdateApplicationStatusRequest(
    int ApplicationId,
    ApplicationStatus Status
);