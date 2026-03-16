using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Forums.Commands.CreateAnswer;

public record CreateAnswerCommand(
    int QuestionId,
    string Content,
    int AuthorId
) : IRequest<StandardResponse<int>>;
