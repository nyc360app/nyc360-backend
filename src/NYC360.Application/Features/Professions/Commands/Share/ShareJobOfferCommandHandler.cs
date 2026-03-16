using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Professions.Commands.Share;

public class ShareJobOfferCommandHandler(
    IProfessionsRepository professionsRepository,
    IPostRepository postRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<ShareJobOfferCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(ShareJobOfferCommand request, CancellationToken ct)
    {
        // 1. Validate the Job Offer exists
        var jobOffer = await professionsRepository.GetJobOfferByIdAsync(request.JobOfferId, ct);
        if (jobOffer == null)
            return StandardResponse<int>.Failure(new ApiError("job.notfound", "The job offer you are trying to share does not exist."));

        // 2. Handle Tags
        var tags = request.Tags != null ? await postRepository.EnsureTagsExistAsync(request.Tags, ct) : new();

        // 3. Create the Social Post
        var post = new Post
        {
            AuthorId = request.UserId,
            Title = jobOffer.Title, // Inherit title from the resource
            Content = request.Content,
            Category = Category.Professions, // Division 8
            PostType = PostType.Job,
            LocationId = jobOffer.Address?.LocationId, // CRITICAL: Link to the Job's neighborhood
            CommunityId = request.CommunityId,
            CreatedAt = DateTime.UtcNow,
            IsApproved = true,
            Tags = tags
        };

        await postRepository.AddAsync(post, ct);

        // 4. Create the Polymorphic Link (The Pointer)
        post.Link = new PostLink
        {
            PostId = post.Id,
            LinkedEntityId = jobOffer.Id,
            Type = PostType.Job
        };

        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<int>.Success(post.Id);
    }
}