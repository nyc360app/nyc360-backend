using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using MediatR;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Commands.DashboardEdit;

public class PostUpdateDashboardCommandHandler(
    IPostRepository postRepository,
    ITopicRepository topicRepository,
    ILocalStorageService storageService,
    IUnitOfWork unitOfWork
) : IRequestHandler<PostUpdateDashboardCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(PostUpdateDashboardCommand request, CancellationToken cancellationToken)
    {
        var post = await postRepository.GetByIdAsync(request.Id, cancellationToken);
        if (post is null)
            return StandardResponse.Failure(new("post.notfound", "Post not found."));

        post.Title = request.Title;
        post.Content = request.Content;
        post.Category = request.Category;

        if (request.TopicId.HasValue && request.TopicId.Value != 0)
        {
            var topic = await topicRepository.GetByIdAsync(request.TopicId.Value, cancellationToken);
            if (topic == null)
                return StandardResponse.Failure(new("post.topic_notfound", "Topic not found."));

            if (topic.Category.HasValue && topic.Category.Value != request.Category)
                return StandardResponse.Failure(new("post.topic_category_mismatch", "Topic category does not match post category."));

            post.TopicId = request.TopicId.Value;
        }
        else
        {
            post.TopicId = null;
        }

        post.LastUpdated = DateTime.UtcNow;

        if (request.AddedAttachments is not null && request.AddedAttachments.Count != 0)
        {
            foreach (var attachment in request.AddedAttachments)
            {
                var fileName = await storageService.SaveFileAsync(attachment, "posts", cancellationToken);
                post.Attachments.Add(new PostAttachment { Url = "@local://" + fileName });
            }
        }

        if (request.RemovedAttachments is not null && request.RemovedAttachments.Count != 0)
        {
            var attachmentsToRemove = post.Attachments
                .Where(a => request.RemovedAttachments.Contains(a.Id))
                .ToList();
            foreach (var attachment in attachmentsToRemove)
            {
                storageService.DeleteFile(attachment.Url.Replace("@local://", ""), "posts");
                post.Attachments.Remove(attachment);
            }
        }

        postRepository.Update(post);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return StandardResponse.Success();
    }
}