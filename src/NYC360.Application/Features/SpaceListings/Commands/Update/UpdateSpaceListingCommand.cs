using Microsoft.AspNetCore.Http;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.SpaceListings;
using NYC360.Domain.Wrappers;
using MediatR;
using NYC360.Application.Features.SpaceListings.Commands.Submit;

namespace NYC360.Application.Features.SpaceListings.Commands.Update;

public record UpdateSpaceListingCommand(
    int ListingId,
    int UserId,
    Category Department,
    SpaceListingEntityType EntityType,
    string Name,
    string Description,
    AddressInputDto Address,
    string? LocationName,
    string? Borough,
    string? Neighborhood,
    string? Website,
    string? PhoneNumber,
    string? PublicEmail,
    string? ContactName,
    string? SubmitterNote,
    bool IsClaimingOwnership,
    List<Category> Categories,
    List<string> Tags,
    Domain.Enums.Users.Industry? BusinessIndustry,
    Domain.Enums.Users.BusinessSize? BusinessSize,
    Domain.Enums.Users.ServiceArea? BusinessServiceArea,
    Domain.Enums.Users.Services? BusinessServices,
    Domain.Enums.Users.OwnershipType? BusinessOwnershipType,
    bool? BusinessIsLicensedInNyc,
    bool? BusinessIsInsured,
    Domain.Enums.Users.OrganizationType? OrganizationType,
    Domain.Enums.Users.OrganizationFundType? OrganizationFundType,
    List<Domain.Enums.Users.OrganizationServices> OrganizationServices,
    bool? OrganizationIsTaxExempt,
    bool? OrganizationIsNysRegistered,
    List<SpaceListingSocialLinkInput> SocialLinks,
    List<SpaceListingHourInput> Hours,
    List<IFormFile>? Images,
    List<IFormFile>? Documents,
    List<IFormFile>? OwnershipDocuments,
    List<IFormFile>? ProofDocuments
) : IRequest<StandardResponse>;
