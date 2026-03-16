using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using MediatR;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Commands.UserEdit;

public class PostUpdateUserCommandHandler(
    IPostRepository postRepository,
    ITagRepository tagRepository, // Added to verify tags if needed
    ITopicRepository topicRepository,
    ILocalStorageService storageService,
    IUnitOfWork unitOfWork
) : IRequestHandler<PostUpdateUserCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(PostUpdateUserCommand request, CancellationToken ct)
    {
        // 1. Fetch Post with Attachments and Tags included
        // Ensure your Repository has a method that includes these navigation properties
        var post = await postRepository.GetByIdAsync(request.PostId, ct);
        
        if (post is null)
            return StandardResponse.Failure(new("post.notfound", "Post not found."));

        // 2. Authorization Check
        if (post.AuthorId != request.UserId)
            return StandardResponse.Failure(new("post.forbidden", "You cannot edit this post."));

        // 3. Update Basic Properties
        post.Title = request.Title;
        post.Content = request.Content;
        post.Category = request.Category;

        // Topic Validation
        if (request.TopicId.HasValue && request.TopicId.Value != 0)
        {
            var topic = await topicRepository.GetByIdAsync(request.TopicId.Value, ct);
            if (topic == null)
                return StandardResponse.Failure(new ApiError("posts.topic_invalid", $"The topic with ID {request.TopicId} does not exist."));

            if (topic.Category.HasValue && topic.Category.Value != request.Category)
                return StandardResponse.Failure(new ApiError("posts.topic_category_mismatch", $"The topic category does not match the post category."));

            post.TopicId = request.TopicId.Value;
        }
        else
        {
            post.TopicId = null;
        }

        post.LastUpdated = DateTime.UtcNow;

        // 4. Handle Tags (Sync Logic)
        if (request.TagIds != null)
        {
            // Identify tags to remove (currently in DB but not in request)
            var tagsToRemove = post.Tags
                .Where(t => !request.TagIds.Contains(t.Id))
                .ToList();

            foreach (var tag in tagsToRemove)
                post.Tags.Remove(tag);

            // Identify tags to add (in request but not currently in DB)
            var currentTagIds = post.Tags.Select(t => t.Id).ToList();
            var tagIdsToAdd = request.TagIds.Except(currentTagIds).ToList();

            foreach (var tagId in tagIdsToAdd)
            {
                // We fetch the tag entity to attach it to the post
                var tag = await tagRepository.GetByIdAsync(tagId);
                if (tag != null)
                {
                    post.Tags.Add(tag);
                }
            }
        }

        // 5. Handle Attachment Removals
        if (request.RemovedAttachments is { Count: > 0 })
        {
            var attachmentsToRemove = post.Attachments
                .Where(a => request.RemovedAttachments.Contains(a.Id))
                .ToList();

            foreach (var attachment in attachmentsToRemove)
            {
                // Clean up local storage
                var filePath = attachment.Url.Replace("@local://", "");
                storageService.DeleteFile(filePath, "posts");
                
                post.Attachments.Remove(attachment);
            }
        }

        // 6. Handle New Attachment Uploads
        if (request.AddedAttachments is { Count: > 0 })
        {
            foreach (var file in request.AddedAttachments)
            {
                var fileName = await storageService.SaveFileAsync(file, "posts", ct);
                post.Attachments.Add(new PostAttachment 
                { 
                    Url = "@local://" + fileName,
                    PostId = post.Id 
                });
            }
        }

        // 7. Persist Changes
        postRepository.Update(post);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}