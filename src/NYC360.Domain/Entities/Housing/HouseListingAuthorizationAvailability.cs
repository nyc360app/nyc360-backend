using NYC360.Domain.Enums.Housing;

namespace NYC360.Domain.Entities.Housing;

public class HouseListingAuthorizationAvailability
{
    public int Id { get; set; }
    public int HouseListingAuthorizationId { get; set; }
    
    public AvailabilityType AvailabilityType { get; set; }
    
    public List<DateOnly> Dates { get; set; } = new();
    
    public TimeOnly TimeFrom { get; set; }
    public TimeOnly TimeTo { get; set; }
    
    public HouseListingAuthorization HouseListingAuthorization { get; set; } = null!;
}
