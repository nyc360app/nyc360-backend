namespace NYC360.Domain.Entities.Locations;

public class Location
{
    public int Id { get; set; }
    public string? Borough { get; set; } = string.Empty;
    public string? Code { get; set; } = string.Empty;
    public string? NeighborhoodNet { get; set; } = string.Empty;
    public string? Neighborhood { get; set; } = string.Empty;
    public int ZipCode { get; set; }
    
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
}