namespace NYC360.Domain.Entities.Forums;

public class Forum
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ForumQuestion> Questions { get; set; } = new List<ForumQuestion>();
    public ICollection<ForumModerator> Moderators { get; set; } = new List<ForumModerator>();
}
