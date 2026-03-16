using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Entities;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;
using NYC360.Domain.Entities.Tags;

namespace NYC360.Application.Features.Communities.Commands.CreatePost;

public class CreateCommunityPostCommandHandler(
    ICommunityRepository communityRepository,
    IPostRepository postRepository,
    IUnitOfWork unitOfWork,
    ILocalStorageService fileService) 
    : IRequestHandler<CreateCommunityPostCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(CreateCommunityPostCommand request, CancellationToken ct)
    {
        // 1. Validate Community existence and Membership
        var community = await communityRepository.GetByIdAsync(request.CommunityId, ct);
        if (community == null)
            return StandardResponse<int>.Failure(new ApiError("community.notfound", "Community not found."));

        if (!community.AnyoneCanPost)
        {
            var isMember = await communityRepository.IsMemberAsync(request.CommunityId, request.UserId, ct);
            if (!isMember)
                return StandardResponse<int>.Failure(new ApiError("community.forbidden", "You must be a member of this community to post."));
        }

        // 2. Handle Attachments via File Service
        var attachments = new List<PostAttachment>();
        if (request.Attachments != null && request.Attachments.Count != 0)
        {
            foreach (var file in request.Attachments)
            {
                var url = await fileService.SaveFileAsync(file, "posts", ct);
                attachments.Add(new PostAttachment { Url = url });
            }
        }

        // 3. Handle Tags using your Repository's logic
        var tags = new List<Tag>();
        if (request.Tags != null && request.Tags.Count != 0)
        {
            tags = await postRepository.EnsureTagsExistAsync(request.Tags, ct);
        }

        // 4. Create the Post Entity
        var post = new Post
        {
            Title = request.Title,
            Content = request.Content,
            AuthorId = request.UserId,
            CommunityId = request.CommunityId,
            
            // Inherit location from community for the "Neighborhood Pulse"
            LocationId = community.LocationId,
            
            Category = Category.Community,
            PostType = PostType.Normal,
            SourceType = PostSource.User,
            
            IsApproved = true,
            CreatedAt = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow,
            
            Attachments = attachments,
            Tags = tags
        };

        // 5. Persist via Repository and Unit of Work
        await postRepository.AddAsync(post, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<int>.Success(post.Id);
    }
}