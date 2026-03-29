using Microsoft.AspNetCore.Http;
using NYC360.Domain.Enums;

namespace NYC360.API.Models.News;

public record NewsRssConnectionRequest(
    Category? Category,
    string Url,
    string Name,
    string? Description,
    string? ImageUrl,
    IFormFile? Image,
    string? Language,
    string? SourceWebsite,
    string? SourceCredibility,
    bool AgreementAccepted,
    string? DivisionTag,
    IFormFile? LogoImage,
    string? LogoFileName
);
