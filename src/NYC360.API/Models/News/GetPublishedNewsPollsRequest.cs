namespace NYC360.API.Models.News;

public record GetPublishedNewsPollsRequest(
    int Page = 1,
    int PageSize = 20
);
