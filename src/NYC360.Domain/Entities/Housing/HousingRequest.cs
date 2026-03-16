using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums.Housing;

namespace NYC360.Domain.Entities.Housing;

public class HousingRequest
{
    public int Id { get; set; }
    
    public int HouseInfoId { get; set; }
    public HouseInfo? HouseInfo { get; set; } = null!;
    
    public int? UserId { get; set; }
    public UserProfile? User { get; set; }
    
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public PreferredContactType PreferredContactType { get; set; }
    public HouseholdType HouseholdType { get; set; }
    
    public DateOnly PreferredContactDate { get; set; }
    public TimeOnly PreferredContactTime { get; set; }
    
    public DateOnly MoveInDate { get; set; }
    public DateOnly? MoveOutDate { get; set; }
    
    public DateOnly? ScheduleVirtualDate { get; set; }
    public TimeOnly? ScheduleVirtualTimeWindow { get; set; }
    
    public DateOnly? InPersonTourDate { get; set; }
    public TimeOnly? InPersonTourTimeWindow { get; set; }
    
    public string Message { get; set; } = string.Empty;
    
    public HousingRequestStatus Status { get; set; } = HousingRequestStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}