using NYC360.Domain.Dtos.User;

namespace NYC360.Domain.Dtos.Forums;

public record ForumAnswerDto(
    int Id,
    int QuestionId,
    string Content,
    bool IsCorrectAnswer,
    DateTime CreatedAt,
    UserMinimalInfoDto Author
);
