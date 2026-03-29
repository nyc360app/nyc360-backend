using NYC360.Domain.Dtos.User;
using NYC360.Domain.Enums;

namespace NYC360.Domain.Dtos.Rss;

public record RssConnectionRequestDto(
    int Id,
    string Url,
    Category Category,
    Category RequestedCategory,
    Category? FinalCategory,
    string Name,
    string? Description,
    string? ImageUrl,
    string? LogoImageUrl,
    string? LogoFileName,
    string? Language,
    string? SourceWebsite,
    string? SourceCredibility,
    bool AgreementAccepted,
    string? DivisionTag,
    RssConnectionStatus Status,
    string? AdminNote,
    int RequesterId,
    string? RequesterName,
    UserMinimalInfoDto? Requester,
    int? ProcessedByUserId,
    DateTime CreatedAt,
    DateTime? ProcessedAt);
