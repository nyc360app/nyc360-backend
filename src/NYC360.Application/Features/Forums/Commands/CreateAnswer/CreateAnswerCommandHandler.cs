using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Forums;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Forums.Commands.CreateAnswer;

public class CreateAnswerCommandHandler(
    IForumAnswerRepository answerRepository,
    IForumQuestionRepository questionRepository
) : IRequestHandler<CreateAnswerCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(CreateAnswerCommand request, CancellationToken cancellationToken)
    {
        var questionExists = await questionRepository.ExistsAsync(x => x.Id == request.QuestionId, cancellationToken);
        if (!questionExists)
            return StandardResponse<int>.Failure(new ApiError("forum.question_not_found", "Question not found."));

        var answer = new ForumAnswer
        {
            QuestionId = request.QuestionId,
            Content = request.Content,
            AuthorId = request.AuthorId,
            CreatedAt = DateTime.UtcNow
        };

        await answerRepository.AddAsync(answer, cancellationToken);
        await answerRepository.SaveChangesAsync(cancellationToken);

        return StandardResponse<int>.Success(answer.Id);
    }
}
