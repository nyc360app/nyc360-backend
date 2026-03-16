using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Entities.Support;

public class TicketMessage
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public string Content { get; set; } = string.Empty;
    
    public int AuthorId { get; set; }
    public UserProfile Author { get; set; } = null!;
    
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsInternalNote { get; set; } // Hidden from the end-user
}