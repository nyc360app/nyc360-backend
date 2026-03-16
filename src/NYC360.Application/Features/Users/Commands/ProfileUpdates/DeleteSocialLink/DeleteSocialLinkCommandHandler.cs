using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.DeleteSocialLink;

public class DeleteSocialLinkCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteSocialLinkCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(DeleteSocialLinkCommand request, CancellationToken ct)
    {
        // 1. Fetch the specific link from the DB
        var link = await userRepository.GetSocialLinkByIdAsync(request.LinkId, ct);
        
        // 2. Security Check: Check if it exists and belongs to the user requesting the delete
        if (link == null || link.UserId != request.UserId)
        {
            return StandardResponse.Failure(
                new ApiError("social_link.not_found", "Social link not found or you do not have permission to delete it.")
            );
        }

        // 3. Perform deletion via the Repository helper
        userRepository.RemoveSocialLink(link);

        // 4. Commit the transaction
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}