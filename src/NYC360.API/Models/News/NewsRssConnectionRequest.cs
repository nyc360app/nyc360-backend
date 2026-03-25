using Microsoft.AspNetCore.Http;

namespace NYC360.API.Models.News;

public record NewsRssConnectionRequest(
    string Url,
    string Name,
    string? Description,
    string? ImageUrl,
    string? Language,
    string? SourceWebsite,
    string? SourceCredibility,
    bool AgreementAccepted,
    string? DivisionTag,
    IFormFile? LogoImage
);
