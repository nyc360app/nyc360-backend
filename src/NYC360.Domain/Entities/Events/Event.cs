using NYC360.Domain.Entities.Locations;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums.Events;

namespace NYC360.Domain.Entities.Events;

public class Event
{
    public int Id { get; set; }

    // Event Details
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public EventCategory Category { get; set; }
    public EventType Type { get; set; }

    // Time & Location
    public DateTime StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }

    public int? AddressId { get; set; }
    public Address? Address { get; set; }

    // Status & Visibility
    public EventStatus Status { get; set; } = EventStatus.Draft;
    public EventVisibility? Visibility { get; set; }
    public string? AccessPasswordHash { get; set; }

    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public int? OwnerId { get; set; }
    public UserProfile? Owner { get; set; }
    
    // hidden
    public bool IsHidden { get; set; }
    public DateTime? HiddenAt { get; set; }
    public int? HiddenByAdminId { get; set; }

    // Navigation
    public ICollection<EventAttachment> Attachments { get; set; } = new List<EventAttachment>();
    public ICollection<EventStaff> Staff { get; set; } = new List<EventStaff>();
    public ICollection<EventTicketTier> Tiers { get; set; } = new List<EventTicketTier>();
    public ICollection<EventAttendance> AttendanceRecords { get; set; } = new List<EventAttendance>();
    public ICollection<EventModerationAction> ModerationActions { get; set; } = new List<EventModerationAction>();

    // Computed
    public bool IsPaid => Tiers.Any(t => t.Price > 0);
}