using System.ComponentModel.DataAnnotations;
using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Commands.SubmitLeaderApplication;

public class SubmitCommunityLeaderApplicationHandler(
    IUserRepository userRepository,
    ICommunityRepository communityRepository,
    ILocalStorageService localStorageService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<SubmitCommunityLeaderApplicationCommand, StandardResponse<CommunityLeaderApplicationSubmissionDto>>
{
    private const int MaxFileSizeBytes = 5 * 1024 * 1024;
    private const string FilesSubfolder = "community-leader-applications";
    private const string PendingApplicationIndexName = "IX_CommunityLeaderApplications_UserId_Pending";

    public async Task<StandardResponse<CommunityLeaderApplicationSubmissionDto>> Handle(
        SubmitCommunityLeaderApplicationCommand request,
        CancellationToken ct)
    {
        // The application layer currently doesn't register a MediatR validation behavior,
        // so this command validates the contract explicitly before persisting anything.
        var validationError = Validate(request);
        if (validationError != null)
            return StandardResponse<CommunityLeaderApplicationSubmissionDto>.Failure(validationError);

        var user = await userRepository.GetProfileByUserIdAsync(request.UserId, ct);
        if (user == null)
        {
            return StandardResponse<CommunityLeaderApplicationSubmissionDto>.Failure(
                new ApiError("user.notFound", "User profile not found."));
        }

        if (await communityRepository.HasPendingLeaderApplicationAsync(request.UserId, ct))
        {
            return StandardResponse<CommunityLeaderApplicationSubmissionDto>.Failure(
                new ApiError(
                    "communityLeaderApplication.pending",
                    "You already have a pending community leader application."));
        }

        string? savedFileName = null;

        try
        {
            savedFileName = await localStorageService.SaveFileAsync(request.VerificationFile!, FilesSubfolder, ct);

            var entity = new CommunityLeaderApplication
            {
                UserId = request.UserId,
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim(),
                PhoneNumber = request.PhoneNumber.Trim(),
                CommunityName = request.CommunityName.Trim(),
                Location = request.Location.Trim(),
                ProfileLink = string.IsNullOrWhiteSpace(request.ProfileLink) ? null : request.ProfileLink.Trim(),
                Motivation = request.Motivation.Trim(),
                Experience = request.Experience.Trim(),
                LedBefore = request.LedBefore,
                WeeklyAvailability = request.WeeklyAvailability.Trim(),
                AgreedToGuidelines = request.AgreedToGuidelines,
                VerificationFileUrl = savedFileName,
                Status = CommunityLeaderApplicationStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await communityRepository.AddLeaderApplicationAsync(entity, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return StandardResponse<CommunityLeaderApplicationSubmissionDto>.Success(
                new CommunityLeaderApplicationSubmissionDto(entity.Id, entity.Status, entity.CreatedAt));
        }
        catch (Exception ex) when (IsPendingDuplicateViolation(ex))
        {
            localStorageService.DeleteFile(savedFileName, FilesSubfolder);

            return StandardResponse<CommunityLeaderApplicationSubmissionDto>.Failure(
                new ApiError(
                    "communityLeaderApplication.pending",
                    "You already have a pending community leader application."));
        }
    }

    private static ApiError? Validate(SubmitCommunityLeaderApplicationCommand request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
            return new ApiError("communityLeaderApplication.fullName.required", "Full name is required.");

        if (request.FullName.Trim().Length > 200)
            return new ApiError("communityLeaderApplication.fullName.invalid", "Full name must be 200 characters or fewer.");

        if (string.IsNullOrWhiteSpace(request.Email))
            return new ApiError("communityLeaderApplication.email.required", "Email is required.");

        var email = request.Email.Trim();
        if (email.Length > 255 || !new EmailAddressAttribute().IsValid(email))
            return new ApiError("communityLeaderApplication.email.invalid", "A valid email address is required.");

        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            return new ApiError("communityLeaderApplication.phoneNumber.required", "Phone number is required.");

        if (request.PhoneNumber.Trim().Length > 50)
            return new ApiError("communityLeaderApplication.phoneNumber.invalid", "Phone number must be 50 characters or fewer.");

        if (string.IsNullOrWhiteSpace(request.CommunityName))
            return new ApiError("communityLeaderApplication.communityName.required", "Community name is required.");

        if (request.CommunityName.Trim().Length > 200)
            return new ApiError("communityLeaderApplication.communityName.invalid", "Community name must be 200 characters or fewer.");

        if (string.IsNullOrWhiteSpace(request.Location))
            return new ApiError("communityLeaderApplication.location.required", "Location is required.");

        if (request.Location.Trim().Length > 200)
            return new ApiError("communityLeaderApplication.location.invalid", "Location must be 200 characters or fewer.");

        if (!string.IsNullOrWhiteSpace(request.ProfileLink))
        {
            var profileLink = request.ProfileLink.Trim();
            if (profileLink.Length > 500 ||
                !Uri.TryCreate(profileLink, UriKind.Absolute, out var profileUri) ||
                (profileUri.Scheme != Uri.UriSchemeHttp && profileUri.Scheme != Uri.UriSchemeHttps))
            {
                return new ApiError("communityLeaderApplication.profileLink.invalid", "Profile link must be a valid absolute URL.");
            }
        }

        if (string.IsNullOrWhiteSpace(request.Motivation))
            return new ApiError("communityLeaderApplication.motivation.required", "Motivation is required.");

        var motivationLength = request.Motivation.Trim().Length;
        if (motivationLength < 20 || motivationLength > 2000)
            return new ApiError("communityLeaderApplication.motivation.invalid", "Motivation must be between 20 and 2000 characters.");

        if (string.IsNullOrWhiteSpace(request.Experience))
            return new ApiError("communityLeaderApplication.experience.required", "Experience is required. If you do not have prior experience, state that explicitly.");

        var experienceLength = request.Experience.Trim().Length;
        if (experienceLength < 10 || experienceLength > 2000)
            return new ApiError("communityLeaderApplication.experience.invalid", "Experience must be between 10 and 2000 characters.");

        if (string.IsNullOrWhiteSpace(request.WeeklyAvailability))
            return new ApiError("communityLeaderApplication.weeklyAvailability.required", "Weekly availability is required.");

        if (request.WeeklyAvailability.Trim().Length > 500)
            return new ApiError("communityLeaderApplication.weeklyAvailability.invalid", "Weekly availability must be 500 characters or fewer.");

        if (!request.AgreedToGuidelines)
            return new ApiError("communityLeaderApplication.guidelines.required", "You must agree to the community guidelines before submitting.");

        if (request.VerificationFile == null)
            return new ApiError("communityLeaderApplication.verificationFile.required", "Verification file is required.");

        if (request.VerificationFile.Length == 0 || request.VerificationFile.Length > MaxFileSizeBytes)
            return new ApiError("communityLeaderApplication.verificationFile.invalid", "Verification file must be greater than 0 bytes and no larger than 5 MB.");

        return null;
    }

    private static bool IsPendingDuplicateViolation(Exception ex)
    {
        Exception? current = ex;

        while (current != null)
        {
            if (current.Message.Contains(PendingApplicationIndexName, StringComparison.OrdinalIgnoreCase))
                return true;

            current = current.InnerException;
        }

        return false;
    }
}
