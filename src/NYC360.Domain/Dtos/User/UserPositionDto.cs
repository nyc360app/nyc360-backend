using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Dtos.User;

public sealed record UserPositionDto(
    string Title,
    string Company,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsCurrent
);

public static class UserPositionDtoExtensions
{
    extension(UserPositionDto)
    {
        public static UserPositionDto Map(UserPosition position) => new(
            position.Title,
            position.Company,
            position.StartDate,
            position.EndDate,
            position.IsCurrent
        );
    }
}