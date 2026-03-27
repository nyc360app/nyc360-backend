using Microsoft.AspNetCore.Http;

namespace NYC360.API.Models.News;

public record UpdateNewsPollRequest(
    string Title,
    string Question,
    string? Description,
    List<string> Options,
    IFormFile? CoverImage,
    DateTime? ClosesAt,
    bool AllowMultipleAnswers = false,
    bool ShowResultsBeforeVoting = false,
    bool IsFeatured = false,
    List<string>? Tags = null,
    int? LocationId = null
);
