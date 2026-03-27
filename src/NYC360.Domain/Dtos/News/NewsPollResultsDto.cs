namespace NYC360.Domain.Dtos.News;

public record NewsPollResultsDto(
    int PollId,
    string Status,
    int TotalVotes,
    bool ShowResultsBeforeVoting,
    List<NewsPollOptionDto> Options
);
