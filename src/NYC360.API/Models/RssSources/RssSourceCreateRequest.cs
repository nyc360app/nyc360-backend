using NYC360.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace NYC360.API.Models.RssSources;

public record RssSourceCreateRequest(
    string Url, 
    Category Category, 
    string Name, 
    string? Description, 
    string? ImageUrl,
    IFormFile? Image);