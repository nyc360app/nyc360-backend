namespace NYC360.Domain.Entities.Housing;

public class HouseListingAuthorizationAttachment
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public int HouseListingAuthorizationId { get; set; }
    public HouseListingAuthorization? HouseListingAuthorization { get; set; }
}