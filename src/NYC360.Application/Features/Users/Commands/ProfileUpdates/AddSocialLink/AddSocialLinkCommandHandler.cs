using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.AddSocialLink;

public class AddSocialLinkCommandHandler(
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork) 
    : IRequestHandler<AddSocialLinkCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(AddSocialLinkCommand request, CancellationToken ct)
    {
        var profile = await userRepository.GetProfileByUserIdAsync(request.UserId, ct);
        if (profile == null) 
            return StandardResponse<int>.Failure(new ApiError("profile.not_found", "Profile not found."));

        var link = new UserSocialLink
        {
            UserId = request.UserId,
            Platform = request.Platform,
            Url = request.Url
        };

        profile.SocialLinks.Add(link);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<int>.Success(link.Id);
    }
}