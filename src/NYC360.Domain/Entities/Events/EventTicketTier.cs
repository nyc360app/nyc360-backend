namespace NYC360.Domain.Entities.Events;

public class EventTicketTier
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Pricing and Capacity
    public decimal? Price { get; set; }
    public int QuantityAvailable { get; set; }
    public int QuantitySold { get; set; }
    
    public int? MinPerOrder { get; set; }
    public int? MaxPerOrder { get; set; }

    public DateTime? SaleStart { get; set; }
    public DateTime? SaleEnd { get; set; }

    public bool IsActive { get; set; } = true;
    
    public ICollection<EventTicket> Tickets { get; set; } = new List<EventTicket>();
}