using Microsoft.AspNetCore.Http;

namespace NYC360.Application.Contracts.Services;

public record NewsPollCreateInput(
    string Title,
    string Question,
    string? Description,
    List<string> Options,
    IFormFile? CoverImage,
    DateTime? ClosesAt,
    bool AllowMultipleAnswers,
    bool ShowResultsBeforeVoting,
    bool IsFeatured,
    List<string>? Tags,
    int? LocationId
);

public record NewsPollUpdateInput(
    string Title,
    string Question,
    string? Description,
    List<string> Options,
    IFormFile? CoverImage,
    DateTime? ClosesAt,
    bool AllowMultipleAnswers,
    bool ShowResultsBeforeVoting,
    bool IsFeatured,
    List<string>? Tags,
    int? LocationId
);
