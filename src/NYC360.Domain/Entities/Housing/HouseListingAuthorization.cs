using NYC360.Domain.Enums.Housing;
using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Entities.Housing;

public class HouseListingAuthorization
{
    public int Id { get; set; }
    public int HouseInfoId { get; set; }
    
    public string FullName { get; set; } = string.Empty;
    public string? OrganizationName { get; set; }
    
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
        
    public ICollection<HouseListingAuthorizationAvailability> Availabilities { get; set; } = new List<HouseListingAuthorizationAvailability>();
    
    public AuthorizationType AuthorizationType { get; set; }
    public ListingAuthorizationDocument ListingAuthorizationDocument { get; set; }
    public DateOnly? AuthorizationValidationDate { get; set; }
    public bool SaveThisAuthorizationForFutureListings { get; set; }
    
    public int UserId { get; set; }
    public UserProfile? User { get; set; }
    
    public ICollection<HouseListingAuthorizationAttachment> Attachments { get; set; } = new List<HouseListingAuthorizationAttachment>();
}