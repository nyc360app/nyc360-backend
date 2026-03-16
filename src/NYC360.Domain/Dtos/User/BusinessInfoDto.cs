using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums.Users;

namespace NYC360.Domain.Dtos.User;

public record BusinessInfoDto(
    Industry Industry,
    BusinessSize BusinessSize,
    ServiceArea ServiceArea,
    Services Services,
    OwnershipType OwnershipType,
    bool IsPublic,
    bool IsLicensedInNyc,
    bool IsInsured
);

public static class BusinessInfoDtoExtensions
{
    extension(BusinessInfoDto)
    {
        public static BusinessInfoDto Map(BusinessInfo info) => new(
            info.Industry,
            info.BusinessSize,
            info.ServiceArea,
            info.Services,
            info.OwnershipType,
            info.IsPublic,
            info.IsLicensedInNyc,
            info.IsInsured
        );
    }
}