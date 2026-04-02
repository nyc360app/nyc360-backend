namespace NYC360.API.Models.News;

public record GetFeaturedNewsRequest(
    int PageSize = 10,
    int Page = 1,
    string? Cursor = null
);
