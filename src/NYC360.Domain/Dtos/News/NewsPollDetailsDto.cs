namespace NYC360.Domain.Dtos.News;

public record NewsPollDetailsDto(
    int PollId,
    string Slug,
    string Title,
    string Question,
    string? Description,
    string? CoverImageUrl,
    string Status,
    bool AllowMultipleAnswers,
    bool ShowResultsBeforeVoting,
    bool IsFeatured,
    int CreatorUserId,
    int? LocationId,
    List<string> Tags,
    DateTime CreatedAt,
    DateTime? ClosesAt,
    bool HasVoted,
    List<int> SelectedOptionIds,
    bool CanEdit,
    bool CanVote,
    int TotalVotes,
    List<NewsPollOptionDto> Options
);
