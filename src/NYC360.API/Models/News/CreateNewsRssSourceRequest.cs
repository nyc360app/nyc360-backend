using Microsoft.AspNetCore.Http;

namespace NYC360.API.Models.News;

public record CreateNewsRssSourceRequest(
    string Url,
    string Name,
    string? Description,
    string? ImageUrl,
    IFormFile? Image
);
