using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums.Users;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Authentication.Commands.Register.Business;

public record BusinessRegisterCommand(
    string Name,
    string Username,
    string Email,
    string Password,
    Industry Industry,
    BusinessSize BusinessSize,
    AddressInputDto Address,
    ServiceArea ServiceArea,
    bool MakeProfilePublic,
    Services Services,
    string? Website,
    string? PhoneNumber,
    string? Description,
    List<UserSocialLinkInputDto> SocialLinks,
    List<Category> Interests,
    bool IsLicensedInNyc,
    bool IsInsured,
    OwnershipType OwnershipType
) : IRequest<StandardResponse>;