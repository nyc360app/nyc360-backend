using NYC360.Domain.Enums.Housing;
using Microsoft.AspNetCore.Http;

namespace NYC360.API.Models.Housing;

public record AvailabilitySlotRequest(
    AvailabilityType AvailabilityType,
    List<DateOnly> Dates,
    TimeOnly TimeFrom,
    TimeOnly TimeTo
);

public record CreateHouseListingAuthorizationRequest(
    int HouseListingId,
    string FullName,
    string? OrganizationName,
    string Email,
    string? PhoneNumber,
    string Availability,
    AuthorizationType AuthorizationType,
    ListingAuthorizationDocument ListingAuthorizationDocument,
    DateOnly? AuthorizationValidationDate,
    bool SaveThisAuthorizationForFutureListings,
    List<IFormFile>? Attachments
);
