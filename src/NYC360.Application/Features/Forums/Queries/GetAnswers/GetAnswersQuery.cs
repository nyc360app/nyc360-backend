using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Forums;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Forums.Queries.GetAnswers;

public record GetAnswersQuery(int QuestionId) : IRequest<StandardResponse<IEnumerable<ForumAnswerDto>>>;

public class GetAnswersQueryHandler(IForumAnswerRepository answerRepository)
    : IRequestHandler<GetAnswersQuery, StandardResponse<IEnumerable<ForumAnswerDto>>>
{
    public async Task<StandardResponse<IEnumerable<ForumAnswerDto>>> Handle(GetAnswersQuery request, CancellationToken cancellationToken)
    {
        var answers = await answerRepository.GetAnswersByQuestionIdAsync(request.QuestionId, cancellationToken);
        
        var dtos = answers.Select(x => new ForumAnswerDto(
            x.Id,
            x.QuestionId,
            x.Content,
            x.IsCorrectAnswer,
            x.CreatedAt,
            new UserMinimalInfoDto(
                x.Author.UserId,
                x.Author.User!.UserName!,
                x.Author.GetFullName(),
                x.Author.AvatarUrl,
                x.Author.User!.Type
            )
        ));

        return StandardResponse<IEnumerable<ForumAnswerDto>>.Success(dtos);
    }
}
