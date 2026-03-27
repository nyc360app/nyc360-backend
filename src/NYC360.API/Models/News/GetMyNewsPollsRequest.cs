namespace NYC360.API.Models.News;

public record GetMyNewsPollsRequest(
    int Page = 1,
    int PageSize = 20,
    string? Status = null
);
