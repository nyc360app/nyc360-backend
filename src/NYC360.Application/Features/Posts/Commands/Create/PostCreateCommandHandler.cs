using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Application.Contracts.Storage;
using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Constants;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Posts.Commands.Create;

public class PostCreateCommandHandler(
    IPostRepository postRepository,
    ILocationRepository locationRepository,
    ITagRepository tagRepository,
    ITopicRepository topicRepository,
    INewsAuthorizationService newsAuthorizationService,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IUnitOfWork unitOfWork,
    ILocalStorageService storageService)
    : IRequestHandler<PostCreateCommand, StandardResponse<PostDto>>
{
    public async Task<StandardResponse<PostDto>> Handle(PostCreateCommand request, CancellationToken cancellationToken)
    {
        // 1. User and Role Validation
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return StandardResponse<PostDto>.Failure(new ApiError("auth.notfound", "User not found."));
        }
        
        var roles = await userManager.GetRolesAsync(user);
        var roleName = roles.FirstOrDefault();
        if (roleName == null)
        {
            return StandardResponse<PostDto>.Failure(new ApiError("auth.role_missing", "User has no role assigned."));
        }

        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            return StandardResponse<PostDto>.Failure(new ApiError("auth.role_invalid", "Assigned role not found."));
        }
        
        // Checks if the user's profile contains a tag belonging to the requested Category/Division
        bool isStaff = roleName == "SuperAdmin" || roleName == "SuccessAdmin" || roleName == "Admin";
        bool isEligible = isStaff;
        bool autoApprove = true;

        if (request.Category == Domain.Enums.Category.News)
        {
            var newsAccess = await newsAuthorizationService.GetAccessAsync(request.UserId, cancellationToken);
            if (newsAccess == null || !newsAccess.CanSubmitContent)
            {
                return StandardResponse<PostDto>.Failure(
                    new ApiError("news.forbidden", "You do not have a verified News department badge to submit news content."));
            }

            if (request.Type != PostType.News)
            {
                return StandardResponse<PostDto>.Failure(
                    new ApiError("news.invalid_post_type", "News division posts must use the News post type."));
            }

            isEligible = true;
            autoApprove = newsAccess.CanPublishContent;
        }

        if (!isEligible && request.Category == Category.Community)
        {
            var hasCommunityLeaderTag = await tagRepository.UserHasTagAsync(
                request.UserId,
                CommunityVerificationTags.ApplyForCommunityLeaderBadgesName,
                cancellationToken);
            var hasCommunityOrganizationTag = await tagRepository.UserHasTagAsync(
                request.UserId,
                CommunityVerificationTags.ListCommunityOrganizationInSpaceName,
                cancellationToken);
            isEligible = hasCommunityLeaderTag || hasCommunityOrganizationTag;
        }

        if (!isEligible && request.Category != Category.News)
        {
            isEligible = await tagRepository.UserHasTagForDivisionAsync(request.UserId, request.Category, cancellationToken);
        }

        if (!isEligible)
        {
            return StandardResponse<PostDto>.Failure(
                new ApiError("posts.unauthorized_division", 
                    $"You do not have the required profile tags to post in the {request.Category} division."));
        }

        // 2. Content Length Check
        if (request.Content.Length > role.ContentLimit)
        {
            return StandardResponse<PostDto>.Failure(
                new ApiError("posts.content.too_long", $"Limit is {role.ContentLimit} characters."));
        }
        
        
        if (request.LocationId != null && request.LocationId != 0)
        {
            var locationExists = await locationRepository.ExistsAsync(request.LocationId.Value, cancellationToken);
            if (!locationExists)
            {
                return StandardResponse<PostDto>.Failure(
                    new ApiError("posts.location_invalid", $"The location with ID {request.LocationId} does not exist."));
            }
        }

        // 3. Topic Validation
        if (request.TopicId.HasValue && request.TopicId.Value != 0)
        {
            var topic = await topicRepository.GetByIdAsync(request.TopicId.Value, cancellationToken);
            if (topic == null)
            {
                return StandardResponse<PostDto>.Failure(
                    new ApiError("posts.topic_invalid", $"The topic with ID {request.TopicId} does not exist."));
            }

            if (topic.Category.HasValue && topic.Category.Value != request.Category)
            {
                return StandardResponse<PostDto>.Failure(
                    new ApiError("posts.topic_category_mismatch", 
                        $"The topic '{topic.Name}' belongs to {topic.Category} and cannot be used in {request.Category}."));
            }
        }
        
        var entity = new Post
        {
            SourceType = PostSource.User,
            PostType = request.Type,
            Category = request.Category,
            TopicId = request.TopicId,
            Title = request.Title,
            Content = request.Content,
            AuthorId = request.UserId,
            LocationId = request.LocationId == 0 ? null : request.LocationId,
            IsApproved = autoApprove,
            ModerationStatus = autoApprove ? PostModerationStatus.Approved : PostModerationStatus.Pending,
            ModeratedAt = autoApprove ? DateTime.UtcNow : null,
            CreatedAt = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow
        };

        // 5. Handle Tags (Ensure they exist in master Tag table)
        if (request.Tags != null && request.Tags.Count != 0)
        {
            foreach (var tag in request.Tags)
            {
                var tagExists = await tagRepository.GetByIdAsync(tag);
                if (tagExists == null)
                {
                    return StandardResponse<PostDto>.Failure(new ApiError("tag.not_found", "Tag not found."));
                }
                entity.Tags.Add(tagExists);
            }
        }

        // 6. Handle Attachments
        if (request.Attachments != null && request.Attachments.Any())
        {
            foreach (var attachment in request.Attachments)
            {
                var fileName = await storageService.SaveFileAsync(attachment, "posts", cancellationToken);
                entity.Attachments.Add(new PostAttachment { Url = "@local://" + fileName });
            }
        }

        // 7. Save to Database
        await postRepository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 8. Return fully mapped DTO
        // We fetch with details to ensure Location and ParentPost DTOs are populated for the response
        var result = await postRepository.GetPostByIdAsync(entity.Id, request.UserId, cancellationToken);
        
        return StandardResponse<PostDto>.Success(result!);
    }
}
