namespace NYC360.Domain.Dtos.News;

public record NewsPollVoteResultDto(
    int PollId,
    int TotalVotes,
    List<int> SelectedOptionIds
);
