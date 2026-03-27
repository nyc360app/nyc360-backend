namespace NYC360.Domain.Dtos.News;

public record NewsPollSummaryDto(
    int PollId,
    string Slug,
    string Title,
    string Question,
    string? Description,
    string? CoverImageUrl,
    string Status,
    bool AllowMultipleAnswers,
    bool ShowResultsBeforeVoting,
    int TotalVotes,
    DateTime CreatedAt,
    DateTime? ClosesAt
);
