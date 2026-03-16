using NYC360.Domain.Entities.Locations;
using NYC360.Domain.Dtos.User;

namespace NYC360.Domain.Dtos.Location;

public record AddressDto(
    int Id,
    string? Street,
    string? BuildingNumber,
    string? ZipCode,
    LocationDto Location,
    UserProfileDto? ManagedByUser
);

public static class AddressDtoExtensions
{
    extension(AddressDto)
    {
        public static AddressDto? Map(Address? address)
        {
            if (address == null) 
                    return null;

            return new AddressDto(
                address.Id,
                address.Street,
                address.BuildingNumber,
                address.ZipCode,
                address.Location != null ? LocationDto.Map(address.Location) : null!, 
                address.ManagedByUser != null ? UserProfileDto.Map(address.ManagedByUser) : null
            );
        }
    }
}