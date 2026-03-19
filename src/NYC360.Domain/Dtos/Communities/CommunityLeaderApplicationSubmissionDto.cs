using NYC360.Domain.Enums.Communities;

namespace NYC360.Domain.Dtos.Communities;

public record CommunityLeaderApplicationSubmissionDto(
    int ApplicationId,
    CommunityLeaderApplicationStatus Status,
    DateTime SubmittedAt
);
