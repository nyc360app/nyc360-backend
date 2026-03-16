using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Forums.Commands.UpdateForum;

public class UpdateForumCommandHandler(
    IForumRepository forumRepository,
    ILocalStorageService localStorageService,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateForumCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateForumCommand request, CancellationToken cancellationToken)
    {
        var forum = await forumRepository.GetByIdAsync(request.Id, cancellationToken);
        if (forum == null)
        {
            return StandardResponse.Failure(new ApiError("forum.not_found", "Forum not found."));
        }

        // Check if slug is taken by another forum
        var isSlugTaken = await forumRepository.ExistsAsync(x => x.Slug == request.Slug && x.Id != request.Id, cancellationToken);
        if (isSlugTaken)
        {
            return StandardResponse.Failure(new ApiError("forum.slug_taken", "The provided slug is already in use by another forum."));
        }

        if (request.IconFile != null)
        {
            if (!string.IsNullOrEmpty(forum.IconUrl))
            {
                localStorageService.DeleteFile(forum.IconUrl, "forums");
            }
            forum.IconUrl = await localStorageService.SaveFileAsync(request.IconFile, "forums", cancellationToken);
        }

        forum.Title = request.Title;
        forum.Description = request.Description;
        forum.Slug = request.Slug.ToLower().Replace(" ", "-");
        forum.IsActive = request.IsActive;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return StandardResponse.Success();
    }
}
