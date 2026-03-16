using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Forums;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Forums.Queries.GetQuestions;

public class GetQuestionsQueryHandler(
    IForumQuestionRepository questionRepository,
    IForumRepository forumRepository)
    : IRequestHandler<GetQuestionsQuery, StandardResponse<ForumWithQuestionsDto>>
{
    public async Task<StandardResponse<ForumWithQuestionsDto>> Handle(GetQuestionsQuery request, CancellationToken ct)
    {
        var forum = await forumRepository.GetBySlugAsync(request.ForumSlug, ct);
        if (forum == null)
            return StandardResponse<ForumWithQuestionsDto>.Failure(new ApiError("NotFound", "Forum not found"));

        var (questions, total) = await questionRepository.GetPagedQuestionsAsync(request.ForumSlug, request.Page, request.PageSize, ct);
        
        var dtos = questions.Select(x => new ForumQuestionDto(
            x.Id,
            x.ForumId,
            x.Title,
            x.Content,
            x.Slug,
            x.IsLocked,
            x.IsPinned,
            x.CreatedAt,
            new UserMinimalInfoDto(
                x.Author.UserId,
                x.Author.User!.UserName!,
                x.Author.GetFullName(),
                x.Author.AvatarUrl,
                x.Author.User!.Type
            ),
            x.Answers.Count
        ));

        var pagedResponse = PagedResponse<ForumQuestionDto>.Create(dtos, request.Page, request.PageSize, total);
        var forumDto = ForumDto.Map(forum, total);
        var result = new ForumWithQuestionsDto(forumDto, pagedResponse);

        return StandardResponse<ForumWithQuestionsDto>.Success(result);
    }
}
