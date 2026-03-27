namespace NYC360.Domain.Dtos.News;

public record NewsPollAdminPendingDto(
    int PollId,
    string Slug,
    string Title,
    string Question,
    string? Description,
    string? CoverImageUrl,
    int CreatorUserId,
    string CreatorName,
    DateTime CreatedAt,
    DateTime? ClosesAt,
    bool AllowMultipleAnswers,
    bool ShowResultsBeforeVoting,
    int OptionCount
);
