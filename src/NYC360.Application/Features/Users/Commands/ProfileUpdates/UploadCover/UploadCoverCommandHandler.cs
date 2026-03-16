using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.UploadCover;

public class UploadCoverCommandHandler(
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork,
    ILocalStorageService fileService) 
    : IRequestHandler<UploadCoverCommand, StandardResponse<string>>
{
    public async Task<StandardResponse<string>> Handle(UploadCoverCommand request, CancellationToken ct)
    {
        // 1. Fetch User
        var user = await userRepository.GetProfileByUserIdAsync(request.UserId, ct);
        if (user == null) 
            return StandardResponse<string>.Failure(new ApiError("user.not_found", "User not found."));

        // 2. Delete old cover from storage
        if (!string.IsNullOrEmpty(user.CoverImageUrl))
        {
             fileService.DeleteFile(user.CoverImageUrl, "covers");
        }

        // 3. Upload new cover to 'covers' folder
        var uploadResult = await fileService.SaveFileAsync(request.File, "covers", ct);

        // 4. Update Database
        user.CoverImageUrl = uploadResult;
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<string>.Success(uploadResult);
    }
}