using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums.Tags;
using NYC360.Domain.Enums;

namespace NYC360.Domain.Entities.Tags;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TagType Type { get; set; }
    
    // For Hierarchical Tags (e.g., Master Role -> Sub-Role)
    public int? ParentTagId { get; set; }
    public Tag? ParentTag { get; set; }
    
    public Category? Division { get; set; } 
    
    // Relationship
    public ICollection<UserProfile> Users { get; set; } = new List<UserProfile>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Tag> ChildTags { get; set; } = new List<Tag>();
}