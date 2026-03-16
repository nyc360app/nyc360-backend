using NYC360.Domain.Enums;

namespace NYC360.Domain.Dtos.Posts;

public record PostFlagAdminDto(
    int Id,
    int PostId,
    string? PostTitle,
    string PostContentSnippet,
    int UserId,
    string Username,
    FlagReasonType Reason,
    string? Details,
    DateTime CreatedAt,
    FlagStatus Status
);