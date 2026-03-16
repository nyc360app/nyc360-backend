using NYC360.Domain.Enums;

namespace NYC360.API.Models.RssSources;

public record RssSourceUpdateRequest(
    int Id, 
    string RssUrl, 
    Category Category, 
    string? Name, 
    string? Description, 
    bool IsActive,
    IFormFile? Image = null
);