using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Entities.Forums;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Forums.Commands.CreateForum;

public class CreateForumCommandHandler(
    IForumRepository forumRepository,
    ILocalStorageService localStorageService
) : IRequestHandler<CreateForumCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(CreateForumCommand request, CancellationToken cancellationToken)
    {
        var isSlugTaken = await forumRepository.ExistsAsync(x => x.Slug == request.Slug, cancellationToken);
        if (isSlugTaken)
        {
            return StandardResponse<int>.Failure(new ApiError("forum.slug_taken", "The provided slug is already in use."));
        }

        string? iconUrl = null;
        if (request.IconFile != null)
        {
            iconUrl = await localStorageService.SaveFileAsync(request.IconFile, "forums", cancellationToken);
        }

        var forum = new Forum
        {
            Title = request.Title,
            Description = request.Description,
            IconUrl = iconUrl,
            Slug = request.Slug.ToLower().Replace(" ", "-"), // Basic normalization
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await forumRepository.AddAsync(forum, cancellationToken);
        await forumRepository.SaveChangesAsync(cancellationToken);

        return StandardResponse<int>.Success(forum.Id);
    }
}
