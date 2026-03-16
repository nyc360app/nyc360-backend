using NYC360.Domain.Enums.Users;
using NYC360.Domain.Enums;

namespace NYC360.API.Models.Authentication.Register;

public record VisitorRegisterRequest(
    string FirstName, 
    string LastName, 
    string Username,
    string Email, 
    string Password,
    string? CityOfOrigin,
    string? CountryOfOrigin,
    VisitPurpose VisitPurpose,
    VisitingLengthOfStay LengthOfStay,
    List<Category> Interests,
    bool ReceiveEventAndCultureRecommendations,
    bool EnableLocationBasedSuggestions,
    bool SavePlacesEventsGuides,
    bool DiscoverableProfile,
    bool AllowMessagesFromNycPartners
);