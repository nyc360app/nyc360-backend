namespace NYC360.API.Models.Forums;

public record CreateAnswerRequest(
    int QuestionId,
    string Content
);
