using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Entities.Forums;

public class ForumQuestion
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    
    public bool IsLocked { get; set; }
    public bool IsPinned { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastUpdatedAt { get; set; }

    public int ForumId { get; set; }
    public Forum Forum { get; set; } = null!;

    public int AuthorId { get; set; }
    public UserProfile Author { get; set; } = null!;

    public ICollection<ForumAnswer> Answers { get; set; } = new List<ForumAnswer>();
}
