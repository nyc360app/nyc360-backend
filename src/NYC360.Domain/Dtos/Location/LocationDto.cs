using NYC360.Domain.Enums;

namespace NYC360.Domain.Dtos.Location;

public record LocationDto(
    int? Id,
    string? Borough,
    string? Code,
    string? NeighborhoodNet,
    string? Neighborhood,
    int ZipCode
);

public static class LocationDtoExtensions
{
    extension(LocationDto)
    {
        public static LocationDto Map(Entities.Locations.Location location) => new(
            location.Id,
            location.Borough,
            location.Code,
            location.NeighborhoodNet,
            location.Neighborhood,
            location.ZipCode
        );
    }
}