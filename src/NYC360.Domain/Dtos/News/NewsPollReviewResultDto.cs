namespace NYC360.Domain.Dtos.News;

public record NewsPollReviewResultDto(
    int PollId,
    string Status,
    string? ModerationNote
);
