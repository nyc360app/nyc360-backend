using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums.Users;

namespace NYC360.Domain.Dtos.User;

public record VisitorInfoDto(
    string? CityOfOrigin,
    string? CountryOfOrigin,
    VisitPurpose VisitPurpose,
    VisitingLengthOfStay LengthOfStay
);

public static class VisitorInfoDtoExtensions
{
    extension(VisitorInfoDto)
    {
        public static VisitorInfoDto Map(VisitorInfo info) => new(
            info.CityOfOrigin,
            info.CountryOfOrigin,
            info.VisitPurpose,
            info.LengthOfStay
        );
    }
}