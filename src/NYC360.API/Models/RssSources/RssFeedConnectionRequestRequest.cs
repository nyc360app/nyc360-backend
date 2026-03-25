using NYC360.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace NYC360.API.Models.RssSources;

public record RssFeedConnectionRequestRequest(
    string Url, 
    Category Category, 
    string Name, 
    string? Description, 
    string? ImageUrl,
    string? Language,
    string? SourceWebsite,
    string? SourceCredibility,
    bool AgreementAccepted,
    string? DivisionTag,
    IFormFile? LogoImage);
