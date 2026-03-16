using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums.Users;

namespace NYC360.Domain.Dtos.User;

public record OrganizationInfoDto(
    OrganizationType OrganizationType,
    ServiceArea ServiceArea,
    OrganizationFundType FundType,
    bool IsTaxExempt,
    bool IsNysRegistered,
    List<OrganizationServices> Services
);

public static class OrganizationInfoDtoExtensions
{
    extension(OrganizationInfoDto)
    {
        public static OrganizationInfoDto Map(OrganizationInfo info) => new(
            info.OrganizationType,
            info.ServiceArea,
            info.FundType,
            info.IsTaxExempt,
            info.IsNysRegistered,
            info.Services.Select(s => s.Service).ToList()
        );
    }
}