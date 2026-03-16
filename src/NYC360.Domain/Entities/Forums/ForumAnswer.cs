using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Entities.Forums;

public class ForumAnswer
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsCorrectAnswer { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastUpdatedAt { get; set; }

    public int QuestionId { get; set; }
    public ForumQuestion Question { get; set; } = null!;

    public int AuthorId { get; set; }
    public UserProfile Author { get; set; } = null!;
}
