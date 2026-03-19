using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.News.Commands.ReviewSubmission;

public class ReviewNewsSubmissionCommandHandler(
    IPostRepository postRepository,
    INewsAuthorizationService newsAuthorizationService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ReviewNewsSubmissionCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(ReviewNewsSubmissionCommand request, CancellationToken ct)
    {
        var access = await newsAuthorizationService.GetAccessAsync(request.ModeratorUserId, ct);
        if (access == null || !access.CanModerateContent)
            return StandardResponse.Failure(new ApiError("news.forbidden", "You do not have permission to moderate News submissions."));

        var post = await postRepository.GetByIdAsync(request.PostId, ct);
        if (post == null)
            return StandardResponse.Failure(new ApiError("news.submission_not_found", "News submission not found."));

        if (post.Category != Category.News || post.SourceType != PostSource.User)
            return StandardResponse.Failure(new ApiError("news.invalid_submission", "Only user-submitted News posts can be reviewed here."));

        post.IsApproved = request.Approved;
        post.ModerationStatus = request.Approved ? PostModerationStatus.Approved : PostModerationStatus.Rejected;
        post.ModerationNote = string.IsNullOrWhiteSpace(request.ModerationNote) ? null : request.ModerationNote.Trim();
        post.ModeratedAt = DateTime.UtcNow;
        post.ModeratedByUserId = request.ModeratorUserId;
        post.LastUpdated = DateTime.UtcNow;

        postRepository.Update(post);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}
