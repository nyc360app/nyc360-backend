using System.ComponentModel.DataAnnotations;
using NYC360.Domain.Entities.Locations;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Entities.Communities;

public class Community
{
    [Key]
    public int Id { get; set; }

    // Basic info
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Rules { get; set; }
    public CommunityType? Type { get; set; }
    public string? AvatarUrl { get; set; } = string.Empty;
    public string? CoverUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; } = false;
    public bool IsPrivate { get; set; } = false;
    public bool RequiresApproval { get; set; } = false;
    public bool AnyoneCanPost { get; set; } = false;
    public int MemberCount { get; set; } = 1;
    public int? LocationId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    // Relationships
    public Location? Location { get; set; }
    public ICollection<CommunityMember> Members { get; set; } = new List<CommunityMember>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<CommunityDisbandRequest> DisbandRequests { get; set; } = new List<CommunityDisbandRequest>();
}
