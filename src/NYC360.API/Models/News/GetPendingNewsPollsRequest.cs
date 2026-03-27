namespace NYC360.API.Models.News;

public record GetPendingNewsPollsRequest(
    int Page = 1,
    int PageSize = 20,
    string? Search = null
);
