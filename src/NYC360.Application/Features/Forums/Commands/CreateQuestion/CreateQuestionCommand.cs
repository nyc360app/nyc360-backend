using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Forums.Commands.CreateQuestion;

public record CreateQuestionCommand(
    int ForumId,
    string Title,
    string Content,
    int AuthorId
) : IRequest<StandardResponse<int>>;
