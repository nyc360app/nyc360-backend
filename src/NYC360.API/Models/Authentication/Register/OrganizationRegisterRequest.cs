using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums.Users;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Enums;

namespace NYC360.API.Models.Authentication.Register;

public record OrganizationRegisterRequest(
    string Name,
    string Username,
    string Email, 
    string Password,
    OrganizationType OrganizationType,
    ServiceArea ServiceArea,
    List<OrganizationServices> Services,
    string? Website,
    string? PhoneNumber,
    string? PublicEmail,
    string? Description,
    List<UserSocialLinkInputDto> SocialLinks,
    List<Category> Interests,
    AddressInputDto Address,
    OrganizationFundType FundType,
    bool IsTaxExempt,
    bool IsNysRegistered
);