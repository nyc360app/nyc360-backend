namespace NYC360.API.Models.News;

public record ReviewNewsSubmissionRequest(
    int PostId,
    bool Approved,
    string? ModerationNote
);
