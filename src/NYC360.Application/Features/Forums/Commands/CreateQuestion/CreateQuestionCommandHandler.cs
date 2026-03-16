using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Entities.Forums;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Forums.Commands.CreateQuestion;

public class CreateQuestionCommandHandler(
    IForumQuestionRepository questionRepository,
    IForumRepository forumRepository,
    ISlugService slugService
) : IRequestHandler<CreateQuestionCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var forumExists = await forumRepository.ExistsAsync(x => x.Id == request.ForumId, cancellationToken);
        if (!forumExists)
            return StandardResponse<int>.Failure(new ApiError("forum.not_found", "Forum not found."));

        var slug = await slugService.GenerateUniqueSlugAsync(
            request.Title, 
            s => questionRepository.ExistsAsync(x => x.Slug == s, cancellationToken), 
            cancellationToken);

        var question = new ForumQuestion
        {
            ForumId = request.ForumId,
            Title = request.Title,
            Content = request.Content,
            AuthorId = request.AuthorId,
            Slug = slug,
            CreatedAt = DateTime.UtcNow
        };

        await questionRepository.AddAsync(question, cancellationToken);
        await questionRepository.SaveChangesAsync(cancellationToken);

        return StandardResponse<int>.Success(question.Id);
    }
}
