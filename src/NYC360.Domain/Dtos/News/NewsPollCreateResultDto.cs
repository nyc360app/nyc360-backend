namespace NYC360.Domain.Dtos.News;

public record NewsPollCreateResultDto(
    int PollId,
    string Status,
    string Slug
);
