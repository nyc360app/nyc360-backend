namespace NYC360.API.Models.News;

public record ReviewNewsPollRequest(
    bool Approved,
    string? AdminComment
);
