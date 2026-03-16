using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums.Events;

namespace NYC360.Domain.Entities.Events;

public class EventTicket
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    
    public int TierId { get; set; }
    public EventTicketTier Tier { get; set; } = null!;

    public int UserId { get; set; } // The attendee/purchaser
    public ApplicationUser User { get; set; } = null!;
    
    public string QRCodeString { get; set; } = string.Empty; // Unique token for validation
    public EventTicketStatus Status { get; set; } = EventTicketStatus.Active;
    
    public string? PaymentReference { get; set; } // Stripe session / intent id
    
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
}