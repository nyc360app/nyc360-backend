namespace NYC360.Domain.Entities.SpaceListings;

public class SpaceListingHour
{
    public int Id { get; set; }
    public int SpaceListingId { get; set; }
    public SpaceListing? SpaceListing { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly? OpenTime { get; set; }
    public TimeOnly? CloseTime { get; set; }
    public bool IsClosed { get; set; }
}
