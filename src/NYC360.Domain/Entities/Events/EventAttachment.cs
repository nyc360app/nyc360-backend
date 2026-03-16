namespace NYC360.Domain.Entities.Events;

public class EventAttachment
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public int EventId { get; set; }
    public Event? Event { get; set; }
}