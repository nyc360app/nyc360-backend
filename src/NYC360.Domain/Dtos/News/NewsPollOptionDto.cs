namespace NYC360.Domain.Dtos.News;

public record NewsPollOptionDto(
    int Id,
    string Text,
    int Votes,
    double VotePercent
);
