using NYC360.Domain.Enums.Housing;
using Microsoft.AspNetCore.Http;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Commands.CreateAuthorization;

public record AvailabilitySlotDto(
    AvailabilityType AvailabilityType,
    List<DateOnly> Dates,
    TimeOnly TimeFrom,
    TimeOnly TimeTo
);

public record CreateHouseListingAuthorizationCommand(
    int UserId,
    int HouseListingId,
    string FullName,
    string? OrganizationName,
    string Email,
    string? PhoneNumber,
    List<AvailabilitySlotDto> Availabilities,
    AuthorizationType AuthorizationType,
    ListingAuthorizationDocument ListingAuthorizationDocument,
    DateOnly? AuthorizationValidationDate,
    bool SaveThisAuthorizationForFutureListings,
    List<IFormFile>? Attachments
) : IRequest<StandardResponse<int>>;
