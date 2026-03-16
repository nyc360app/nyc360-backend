using NYC360.Domain.Enums.Events;

namespace NYC360.Domain.Entities.Events;

public class EventModerationAction
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int AdminId { get; set; }

    public ModerationActionType Action { get; set; }
    public string Reason { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}