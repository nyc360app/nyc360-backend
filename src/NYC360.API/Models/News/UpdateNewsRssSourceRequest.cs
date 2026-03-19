using Microsoft.AspNetCore.Http;

namespace NYC360.API.Models.News;

public record UpdateNewsRssSourceRequest(
    int Id,
    string RssUrl,
    string? Name,
    string? Description,
    bool IsActive,
    IFormFile? Image = null
);
