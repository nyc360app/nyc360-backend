namespace NYC360.API.Models.Forums;

public record CreateQuestionRequest(
    int ForumId,
    string Title,
    string Content
);
