using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums;

namespace NYC360.Domain.Entities;

public class RssFeedConnectionRequest
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public Category Category { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    
    public RssConnectionStatus Status { get; set; } = RssConnectionStatus.Pending;
    public string? AdminNote { get; set; }
    
    public int RequesterId { get; set; }
    public UserProfile? Requester { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
}
