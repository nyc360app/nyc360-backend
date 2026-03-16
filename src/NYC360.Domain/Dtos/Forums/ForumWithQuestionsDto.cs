using NYC360.Domain.Wrappers;

namespace NYC360.Domain.Dtos.Forums;

public record ForumWithQuestionsDto(
    ForumDto Forum,
    PagedResponse<ForumQuestionDto> Questions
);
