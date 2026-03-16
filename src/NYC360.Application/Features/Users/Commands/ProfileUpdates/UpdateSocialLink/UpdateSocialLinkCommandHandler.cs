using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.UpdateSocialLink;

public class UpdateSocialLinkCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateSocialLinkCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateSocialLinkCommand request, CancellationToken ct)
    {
        var link = await userRepository.GetSocialLinkByIdAsync(request.LinkId, ct);

        if (link == null || link.UserId != request.UserId)
        {
            return StandardResponse.Failure(
                new ApiError("social_link.not_found", "Social link not found or unauthorized.")
            );
        }

        link.Platform = request.Platform;
        link.Url = request.Url;

        await unitOfWork.SaveChangesAsync(ct);
        return StandardResponse.Success();
    }
}