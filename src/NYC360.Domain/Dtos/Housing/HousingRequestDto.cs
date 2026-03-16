using NYC360.Domain.Entities.Housing;
using NYC360.Domain.Enums.Housing;
using NYC360.Domain.Dtos.User;

namespace NYC360.Domain.Dtos.Housing;

public record HousingRequestDto(
    int Id,
    HousingMinimalDto HousingInfo,
    UserMinimalInfoDto? User,
    string? Name,
    string? Email,
    string? PhoneNumber,
    PreferredContactType PreferredContactType,
    DateOnly PreferredContactDate,
    TimeOnly PreferredContactTime,
    
    HouseholdType HouseholdType,
    
    DateOnly MoveInDate,
    DateOnly? MoveOutDate,
    
    DateOnly? ScheduleVirtualDate,
    TimeOnly? ScheduleVirtualTimeWindow,
    
    DateOnly? InPersonTourDate,
    TimeOnly? InPersonTourTimeWindow,
    string Message,
    HousingRequestStatus Status,
    DateTime CreatedAt
);

public static class HousingRequestDtoExtensions
{
    extension(HousingRequestDto)
    {
        public static HousingRequestDto Map(HousingRequest x) => new(
            x.Id,
            HousingMinimalDto.Map(x.HouseInfo!),
            x.User != null ? UserMinimalInfoDto.Map(x.User) : null,
            x.Name,
            x.Email,
            x.PhoneNumber,
            x.PreferredContactType,
            x.PreferredContactDate,
            x.PreferredContactTime,
            x.HouseholdType,
            x.MoveInDate,
            x.MoveOutDate,
            x.ScheduleVirtualDate,
            x.ScheduleVirtualTimeWindow,
            x.InPersonTourDate,
            x.InPersonTourTimeWindow,
            x.Message,
            x.Status,
            x.CreatedAt
        );
    }
}