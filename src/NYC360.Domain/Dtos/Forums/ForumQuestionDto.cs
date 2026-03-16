using NYC360.Domain.Dtos.User;

namespace NYC360.Domain.Dtos.Forums;

public record ForumQuestionDto(
    int Id,
    int ForumId,
    string Title,
    string Content,
    string Slug,
    bool IsLocked,
    bool IsPinned,
    DateTime CreatedAt,
    UserMinimalInfoDto Author,
    int AnswersCount
);
