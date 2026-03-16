using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Entities.Housing;
using NYC360.Domain.Enums.Housing;

namespace NYC360.Domain.Dtos.Housing;

public record AvailabilitySlotResponseDto(
    AvailabilityType AvailabilityType,
    List<DateOnly> Dates,
    TimeOnly TimeFrom,
    TimeOnly TimeTo
);

public record HouseListingAuthorizationDto(
    int Id,
    int HouseListingId,
    string FullName,
    string? OrganizationName,
    string Email,
    string? PhoneNumber,
    List<AvailabilitySlotResponseDto> Availabilities,
    AuthorizationType AuthorizationType,
    ListingAuthorizationDocument ListingAuthorizationDocument,
    DateOnly? AuthorizationValidationDate,
    bool SaveThisAuthorizationForFutureListings,
    List<AttachmentDto> Attachments
);

public static class HouseListingAuthorizationDtoExtensions
{
    extension(HouseListingAuthorizationDto)
    {
        public static HouseListingAuthorizationDto Map(HouseListingAuthorization auth) => new HouseListingAuthorizationDto(
            auth.Id,
            auth.HouseInfoId,
            auth.FullName,
            auth.OrganizationName,
            auth.Email,
            auth.PhoneNumber,
            auth.Availabilities.Select(x => new AvailabilitySlotResponseDto(
                x.AvailabilityType,
                x.Dates,
                x.TimeFrom,
                x.TimeTo
            )).ToList(),
            auth.AuthorizationType,
            auth.ListingAuthorizationDocument,
            auth.AuthorizationValidationDate,
            auth.SaveThisAuthorizationForFutureListings,
            auth.Attachments.Select(AttachmentDto.Map).ToList()
        );
    }
}