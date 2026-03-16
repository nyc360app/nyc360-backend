namespace NYC360.Domain.Entities.Housing;

public class HousingAttachment
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public int HousingId { get; set; }
    public HouseInfo? Housing { get; set; }
}