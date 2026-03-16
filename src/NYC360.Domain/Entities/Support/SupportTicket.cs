using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums;

namespace NYC360.Domain.Entities.Support;

public class SupportTicket
{
    public int Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SupportTicketStatus Status { get; set; } = SupportTicketStatus.Active;
    
    // user identity
    public string? Email { get; set; }
    public string? Name { get; set; }
    public int? CreatorId { get; set; }
    public UserProfile? Creator { get; set; }
    
    public int? AssignedAdminId { get; set; }
    public UserProfile? AssignedAdmin { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    //public ICollection<TicketMessage> Messages { get; set; } = new List<TicketMessage>();
}