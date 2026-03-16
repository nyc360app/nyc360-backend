using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.UploadAvatar;

public class UploadAvatarCommandHandler(
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork,
    ILocalStorageService fileService) : IRequestHandler<UploadAvatarCommand, StandardResponse<string>>
{
    public async Task<StandardResponse<string>> Handle(UploadAvatarCommand request, CancellationToken ct)
    {
        // 1. Fetch User (ApplicationUser level because ImageUrl is there)
        var user = await userRepository.GetProfileByUserIdAsync(request.UserId, ct);
        if (user == null) 
            return StandardResponse<string>.Failure(new ApiError("user.not_found", "User not found."));

        // 2. Delete old image from storage if it exists
        if (!string.IsNullOrEmpty(user.AvatarUrl))
        {
            fileService.DeleteFile(user.AvatarUrl, "avatars");
        }

        // 3. Upload new image
        // 'avatars' is the folder name/container
        var uploadResult = await fileService.SaveFileAsync(request.File, "avatars", ct);

        // 4. Update Database
        user.AvatarUrl = uploadResult;
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<string>.Success(uploadResult);
    }
}