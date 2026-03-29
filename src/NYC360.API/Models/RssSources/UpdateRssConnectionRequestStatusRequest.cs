using NYC360.Domain.Enums;

namespace NYC360.API.Models.RssSources;

public record UpdateRssConnectionRequestStatusRequest(
    int Id,
    RssConnectionStatus Status,
    string? AdminNote,
    Category? Category = null);
