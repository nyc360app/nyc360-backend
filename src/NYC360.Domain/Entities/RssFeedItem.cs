using NYC360.Domain.Enums;

namespace NYC360.Domain.Entities;

public class RssFeedItem
{
    public int Id { get; set; }
    public int SourceId { get; set; }
    public Category Category { get; set; }
    public string? Guid { get; set; }
    public string Link { get; set; } = string.Empty;
    public string LinkHash { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime FetchedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public string? RawMetadataJson { get; set; }
}
