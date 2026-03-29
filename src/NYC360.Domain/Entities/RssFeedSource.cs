using NYC360.Domain.Enums;

namespace NYC360.Domain.Entities;

public class RssFeedSource
{
    public int Id { get; set; }
    public string? RssUrl { get; set; }
    public Category Category { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime LastChecked { get; set; }
    public DateTime? LastCheckedAt { get; set; }
    public DateTime? LastSuccessAt { get; set; }
    public string? LastError { get; set; }
}
