using System.ComponentModel.DataAnnotations;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Events;

namespace NYC360.Domain.Entities.Events;

public class EventAttendance
{
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public int UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public AttendanceStatus Status { get; set; }
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
}