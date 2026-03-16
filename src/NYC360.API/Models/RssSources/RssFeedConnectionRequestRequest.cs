using NYC360.Domain.Enums;

namespace NYC360.API.Models.RssSources;

public record RssFeedConnectionRequestRequest(
    string Url, 
    Category Category, 
    string Name, 
    string? Description, 
    string? ImageUrl);
