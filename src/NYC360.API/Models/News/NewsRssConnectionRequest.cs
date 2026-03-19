namespace NYC360.API.Models.News;

public record NewsRssConnectionRequest(
    string Url,
    string Name,
    string? Description,
    string? ImageUrl
);
