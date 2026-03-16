using NYC360.Domain.Entities.Housing;

namespace NYC360.Domain.Dtos.Housing;

public record HousingMinimalDto(
    int Id,
    int StartingPrice,
    int? Size,
    int NumberOfRooms,
    int NumberOfBathrooms,
    string ImageUrl
);

public static class HousingMinimalDtoExtensions
{
    extension(HousingMinimalDto)
    {
        public static HousingMinimalDto Map(HouseInfo info) => new(
            info.Id,
            info.StartingPrice,
            info.Size,
            info.Bedrooms,
            info.Bathrooms,
            info.Attachments.FirstOrDefault()?.Url!
        );
    }
}