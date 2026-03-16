using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Forums;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Forums.Queries.GetQuestionDetails;

public record GetQuestionDetailsQuery(int QuestionId) : IRequest<StandardResponse<ForumQuestionDetailsDto>>;

public record ForumQuestionDetailsDto(
    ForumQuestionDto Question,
    IEnumerable<ForumAnswerDto> Answers
);

public class GetQuestionDetailsQueryHandler(
    IForumQuestionRepository questionRepository,
    IForumAnswerRepository answerRepository
) : IRequestHandler<GetQuestionDetailsQuery, StandardResponse<ForumQuestionDetailsDto>>
{
    public async Task<StandardResponse<ForumQuestionDetailsDto>> Handle(GetQuestionDetailsQuery request, CancellationToken cancellationToken)
    {
        var question = await questionRepository.GetQuestionByIdAsync(request.QuestionId, cancellationToken);
        if (question == null)
            return StandardResponse<ForumQuestionDetailsDto>.Failure(new ApiError("forum.question_not_found", "Question not found."));

        var answers = await answerRepository.GetAnswersByQuestionIdAsync(request.QuestionId, cancellationToken);

        var questionDto = new ForumQuestionDto(
            question.Id,
            question.ForumId,
            question.Title,
            question.Content,
            question.Slug,
            question.IsLocked,
            question.IsPinned,
            question.CreatedAt,
            new UserMinimalInfoDto(
                question.Author.UserId,
                question.Author.User!.UserName!,
                question.Author.GetFullName(),
                question.Author.AvatarUrl,
                question.Author.User!.Type
            ),
            question.Answers.Count
        );

        var answerDtos = answers.Select(x => new ForumAnswerDto(
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

        return StandardResponse<ForumQuestionDetailsDto>.Success(new ForumQuestionDetailsDto(questionDto, answerDtos));
    }
}
