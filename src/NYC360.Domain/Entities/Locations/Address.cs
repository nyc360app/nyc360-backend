using NYC360.Domain.Enums.Locations;
using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Entities.Locations;

public class Address
{
    public int Id { get; set; }
    public string? Street { get; set; } = string.Empty;
    public string? BuildingNumber { get; set; } = string.Empty;
    public string? ZipCode { get; set; } = string.Empty;
    public LocationType LocationType { get; set; }
    
    public int LocationId { get; set; }
    public Location? Location { get; set; }
    
    public int? ManagedByUserId { get; set; }
    public UserProfile? ManagedByUser { get; set; }
}