namespace NYC360.Domain.Dtos.News;

public record NewsPollListItemDto(
    int PollId,
    string Slug,
    string Title,
    string Question,
    string Status,
    bool AllowMultipleAnswers,
    bool ShowResultsBeforeVoting,
    bool IsFeatured,
    string? CoverImageUrl,
    int OptionCount,
    int TotalVotes,
    DateTime CreatedAt,
    DateTime? ClosesAt,
    bool HasVoted
);
