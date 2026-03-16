using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Dtos.Professions;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Enums;
using NYC360.Domain.Dtos.Topics;

namespace NYC360.Domain.Dtos.Posts;

public record PostDto(
    int Id, 
    string? Title, 
    string Content,  
    PostSource SourceType, 
    PostType PostType,
    Category Category,
    LocationDto? Location,
    PostDto? ParentPost,
    List<AttachmentDto> Attachments,
    PostStatsDto? Stats,
    DateTime CreatedAt,
    DateTime LastUpdated,
    object? Author,
    List<string> Tags,
    bool IsSavedByUser,
    InteractionType? CurrentUserInteraction,
    object? LinkedResource,
    TopicDto? Topic
);

// Mapping
public static class PostDtoExtensions
{
    extension(PostDto)
    {
        public static PostDto Map(Post post, InteractionType? currentUserInteraction = null)
        {
            object? author = null;
            if (post.Author != null)
            {
                author = UserMinimalInfoDto.Map(post.Author);
            }

            if (post.Source != null)
            {
                author = RssSourcePostDto.Map(post.Source);
            }
            return new PostDto(
                post.Id,
                post.Title,
                post.Content!,
                post.SourceType,
                post.PostType,
                post.Category,
                post.Location != null ? LocationDto.Map(post.Location) : null,
                post.ParentPost != null ? PostDto.Map(post.ParentPost) : null,
                post.Attachments.Select(AttachmentDto.Map).ToList(),
                post.Stats != null ? PostStatsDto.Map(post.Stats!) : null,
                post.CreatedAt,
                post.LastUpdated,
                author,
                post.Tags.Select(t => t.Name).ToList(),
                false,
                currentUserInteraction,
                null,
                post.Topic != null ? new TopicDto 
                { 
                    Id = post.Topic.Id, 
                    Name = post.Topic.Name, 
                    Category = post.Topic.Category 
                } : null
            );
        }
    }
}