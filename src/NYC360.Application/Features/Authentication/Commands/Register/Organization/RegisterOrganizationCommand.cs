using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums.Users;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Authentication.Commands.Register.Organization;

public record RegisterOrganizationCommand (
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
) : IRequest<StandardResponse>;