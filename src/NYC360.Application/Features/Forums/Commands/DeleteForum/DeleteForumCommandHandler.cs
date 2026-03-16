using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Forums.Commands.DeleteForum;

public class DeleteForumCommandHandler(
    IForumRepository forumRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteForumCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(DeleteForumCommand request, CancellationToken cancellationToken)
    {
        var forum = await forumRepository.GetByIdAsync(request.Id, cancellationToken);
        if (forum == null)
        {
            return StandardResponse.Failure(new ApiError("forum.not_found", "Forum not found."));
        }

        forumRepository.Remove(forum);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return StandardResponse.Success();
    }
}
