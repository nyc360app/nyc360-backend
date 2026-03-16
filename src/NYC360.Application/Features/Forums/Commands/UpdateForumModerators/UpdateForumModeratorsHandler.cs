using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Forums.Commands.UpdateForumModerators;

public class UpdateForumModeratorsHandler(
    IForumRepository forumRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateForumModeratorsCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateForumModeratorsCommand request, CancellationToken cancellationToken)
    {
        var forum = await forumRepository.GetByIdWithModeratorsAsync(request.ForumId, cancellationToken);
        if (forum == null)
        {
            return StandardResponse.Failure(new ApiError("Forums.NotFound", "Forum not found"));
        }

        var currentModeratorIds = forum.Moderators.Select(m => m.ModeratorId).ToList();

        var modsToRemove = forum.Moderators.Where(m => !request.ModeratorIds.Contains(m.ModeratorId)).ToList();
        foreach (var mod in modsToRemove)
        {
            forum.Moderators.Remove(mod);
        }

        // Moderators to add
        var newModeratorIds = request.ModeratorIds.Except(currentModeratorIds).ToList();
        foreach (var newModId in newModeratorIds)
        {
            forum.Moderators.Add(new Domain.Entities.Forums.ForumModerator 
            { 
                ForumId = forum.Id, 
                ModeratorId = newModId,
                AssignedAt = DateTime.UtcNow
            });
        }
        
        forumRepository.Update(forum);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return StandardResponse.Success();
    }
}
