using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Constants;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.ReviewLeaderApplication;

public class ReviewCommunityLeaderApplicationCommandHandler(
    ICommunityRepository communityRepository,
    ITagRepository tagRepository,
    IVerificationRepository verificationRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ReviewCommunityLeaderApplicationCommand, StandardResponse<CommunityLeaderApplicationAdminDetailsDto>>
{
    private const int MaxAdminNotesLength = 2000;

    public async Task<StandardResponse<CommunityLeaderApplicationAdminDetailsDto>> Handle(
        ReviewCommunityLeaderApplicationCommand request,
        CancellationToken ct)
    {
        if (!string.IsNullOrWhiteSpace(request.AdminNotes) && request.AdminNotes.Trim().Length > MaxAdminNotesLength)
        {
            return StandardResponse<CommunityLeaderApplicationAdminDetailsDto>.Failure(
                new ApiError("communityLeaderApplication.adminNotes.invalid", "Admin notes must be 2000 characters or fewer."));
        }

        var application = await communityRepository.GetLeaderApplicationByIdAsync(request.ApplicationId, ct);
        if (application == null)
        {
            return StandardResponse<CommunityLeaderApplicationAdminDetailsDto>.Failure(
                new ApiError("communityLeaderApplication.notFound", "Community leader application not found."));
        }

        if (application.Status != CommunityLeaderApplicationStatus.Pending)
        {
            return StandardResponse<CommunityLeaderApplicationAdminDetailsDto>.Failure(
                new ApiError("communityLeaderApplication.alreadyReviewed", "This community leader application has already been reviewed."));
        }

        application.Status = request.Approved
            ? CommunityLeaderApplicationStatus.Approved
            : CommunityLeaderApplicationStatus.Rejected;
        application.AdminNotes = string.IsNullOrWhiteSpace(request.AdminNotes) ? null : request.AdminNotes.Trim();
        application.ReviewedAt = DateTime.UtcNow;

        if (request.Approved)
        {
            var leaderTag = await ResolveLeaderBadgeTagAsync(ct);
            if (leaderTag == null)
            {
                return StandardResponse<CommunityLeaderApplicationAdminDetailsDto>.Failure(
                    new ApiError("communityLeaderApplication.tag.notFound", "Community leader badge tag was not found."));
            }

            var userAlreadyHasTag = await verificationRepository.UserHasSpecificTagAsync(application.UserId, leaderTag.Id, ct);
            if (!userAlreadyHasTag)
            {
                await tagRepository.AddUserTagAsync(new UserTag
                {
                    UserId = application.UserId,
                    TagId = leaderTag.Id
                }, ct);
            }
        }

        communityRepository.UpdateLeaderApplication(application);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<CommunityLeaderApplicationAdminDetailsDto>.Success(
            CommunityLeaderApplicationAdminDtoExtensions.MapDetails(application));
    }

    private async Task<Tag?> ResolveLeaderBadgeTagAsync(CancellationToken ct)
    {
        var tag = await tagRepository.GetByIdAsync(CommunityVerificationTags.ApplyForCommunityLeaderBadgesId);
        if (tag != null)
            return tag;

        return await tagRepository.GetByNameAsync(CommunityVerificationTags.ApplyForCommunityLeaderBadgesName, ct);
    }
}
