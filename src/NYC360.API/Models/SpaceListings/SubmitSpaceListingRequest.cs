using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums;

namespace NYC360.API.Models.SpaceListings;

public class SubmitSpaceListingRequest
{
    // Accept both enum names and numeric values from form-data.
    public string? Department { get; set; }
    public string? EntityType { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    // Nested object style: address.locationId, address.street, ...
    public AddressInputDto? Address { get; set; }

    // Flattened fallback style: locationId, street, ...
    public int? AddressId { get; set; }
    public int? LocationId { get; set; }
    public string? Street { get; set; }
    public string? BuildingNumber { get; set; }
    public string? ZipCode { get; set; }

    public string? LocationName { get; set; }
    public string? Borough { get; set; }
    public string? Neighborhood { get; set; }
    public string? Website { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PublicEmail { get; set; }
    public string? ContactName { get; set; }
    public string? SubmitterNote { get; set; }
    public bool IsClaimingOwnership { get; set; }

    public List<Category>? Categories { get; set; }
    public string? CategoriesJson { get; set; }

    public List<string>? Tags { get; set; }
    public string? TagsJson { get; set; }

    public Domain.Enums.Users.Industry? BusinessIndustry { get; set; }
    public Domain.Enums.Users.BusinessSize? BusinessSize { get; set; }
    public Domain.Enums.Users.ServiceArea? BusinessServiceArea { get; set; }
    public Domain.Enums.Users.Services? BusinessServices { get; set; }
    public Domain.Enums.Users.OwnershipType? BusinessOwnershipType { get; set; }
    public bool? BusinessIsLicensedInNyc { get; set; }
    public bool? BusinessIsInsured { get; set; }
    public Domain.Enums.Users.OrganizationType? OrganizationType { get; set; }
    public Domain.Enums.Users.OrganizationFundType? OrganizationFundType { get; set; }

    public List<Domain.Enums.Users.OrganizationServices>? OrganizationServices { get; set; }
    public string? OrganizationServicesJson { get; set; }

    public bool? OrganizationIsTaxExempt { get; set; }
    public bool? OrganizationIsNysRegistered { get; set; }

    public List<SpaceListingSocialLinkInput>? SocialLinks { get; set; }
    public string? SocialLinksJson { get; set; }

    public List<SpaceListingHourInput>? Hours { get; set; }
    public string? HoursJson { get; set; }

    public bool SaveAsDraft { get; set; }
    public List<IFormFile>? Images { get; set; }
    public List<IFormFile>? Documents { get; set; }
    public List<IFormFile>? OwnershipDocuments { get; set; }
    public List<IFormFile>? ProofDocuments { get; set; }
}
