using NYC360.Domain.Enums.Users;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Authentication.Commands.Register.Visitor;

public record VisitorRegisterCommand(
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
) : IRequest<StandardResponse>;