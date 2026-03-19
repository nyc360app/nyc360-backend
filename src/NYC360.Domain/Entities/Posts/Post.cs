using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Entities.Locations;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Enums;
using NYC360.Domain.Entities.Topics;
using NYC360.Domain.Enums.Users;

namespace NYC360.Domain.Entities.Posts;

public class Post
{
    public int Id { get; set; }
    
    // Content
    public string? Title { get; set; }
    public string? Content { get; set; }
    
    // Metadata
    public PostSource SourceType { get; set; }
    public PostType PostType { get; set; }
    public Category Category { get; set; }
    
    public int? TopicId { get; set; }
    public Topic? Topic { get; set; }
    
    // Stats
    public PostStats? Stats { get; set; } = new PostStats();
    public bool IsApproved { get; set; }
    public PostModerationStatus ModerationStatus { get; set; } = PostModerationStatus.Approved;
    public string? ModerationNote { get; set; }
    public DateTime? ModeratedAt { get; set; }
    public int? ModeratedByUserId { get; set; }
    
    // Date
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdated { get; set; }
    
    // Author
    public int? AuthorId { get; set; }
    public UserType? AuthorType { get; set; }
    public UserProfile? Author { get; set; }
    
    // Source
    public int? SourceId { get; set; }
    public RssFeedSource? Source { get; set; }
    
    // inside Post class
    public int? CommunityId { get; set; }
    public Community? Community { get; set; }
    
    public int? ParentPostId { get; set; }
    public Post? ParentPost { get; set; }
    
    public int? LocationId { get; set; }
    public Location? Location { get; set; }
    
    public PostLink? Link { get; set; } 
    public ICollection<PostAttachment> Attachments { get; set; } = new List<PostAttachment>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public ICollection<PostComment> Comments { get; set; } = new List<PostComment>();
    public ICollection<PostInteraction> Interactions { get; set; } = new List<PostInteraction>();
    public ICollection<PostViewEvent> Views { get; set; } = new List<PostViewEvent>();
}
