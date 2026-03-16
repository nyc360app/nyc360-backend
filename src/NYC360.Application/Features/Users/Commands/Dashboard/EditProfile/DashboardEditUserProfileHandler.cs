using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.Dashboard.EditProfile;

public class DashboardEditUserProfileHandler(
    IUserRepository userRepository,
    ILocalStorageService localStorage,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<DashboardEditUserProfileCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(DashboardEditUserProfileCommand request, CancellationToken ct)
    {
        var user = await userRepository.GetProfileByUserIdAsync(request.Id, ct);
        if (user is null)
            return StandardResponse.Failure(new ApiError("user.notFound", "User not found."));

        // update properties
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Bio = request.Bio;

        if (request.Avatar != null)
        {
            if (string.IsNullOrWhiteSpace(user.AvatarUrl))
            {
                localStorage.DeleteFile(user.AvatarUrl, "avatars");
            }
            user.AvatarUrl = await localStorage.SaveFileAsync(request.Avatar, "avatars", ct);
        }

        await unitOfWork.SaveChangesAsync(ct);
        return StandardResponse.Success();
    }
}