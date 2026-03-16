using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NYC360.Domain.Enums.Users;

namespace NYC360.Domain.Entities.User;

public class VisitorInfo
{
    [Key, ForeignKey(nameof(UserProfile))]
    public int UserId { get; set; }
    
    public string? CityOfOrigin { get; set; }
    public string? CountryOfOrigin { get; set; }
    public VisitPurpose VisitPurpose { get; set; }
    public VisitingLengthOfStay LengthOfStay { get; set; }
    public bool ReceiveEventAndCultureRecommendations { get; set; }
    public bool EnableLocationBasedSuggestions { get; set; }
    public bool SavePlacesEventsGuides { get; set; }
    public bool DiscoverableProfile { get; set; }
    public bool AllowMessagesFromNycPartners { get; set; }
    
    public UserProfile? UserProfile { get; set; }
}