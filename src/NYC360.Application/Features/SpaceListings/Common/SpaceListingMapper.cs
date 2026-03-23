using NYC360.Domain.Dtos.SpaceListings;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Entities.SpaceListings;

namespace NYC360.Application.Features.SpaceListings.Common;

public static class SpaceListingMapper
{
    public static SpaceListingDetailsDto MapDetails(SpaceListing listing)
    {
        return new SpaceListingDetailsDto(
            listing.Id,
            listing.Submitter != null ? UserMinimalInfoDto.Map(listing.Submitter) : null,
            listing.Department,
            listing.EntityType,
            listing.Status,
            listing.OwnershipStatus,
            listing.Name,
            listing.Description,
            listing.LocationId,
            listing.LocationName,
            listing.Borough,
            listing.Neighborhood,
            listing.Street,
            listing.BuildingNumber,
            listing.ZipCode,
            listing.Website,
            listing.PhoneNumber,
            listing.PublicEmail,
            listing.ContactName,
            listing.SubmitterNote,
            listing.IsClaimingOwnership,
            listing.ClaimedByUserId,
            listing.Categories,
            listing.Tags,
            listing.BusinessIndustry,
            listing.BusinessSize,
            listing.BusinessServiceArea,
            listing.BusinessServices,
            listing.BusinessOwnershipType,
            listing.BusinessIsLicensedInNyc,
            listing.BusinessIsInsured,
            listing.OrganizationType,
            listing.OrganizationFundType,
            listing.OrganizationServices,
            listing.OrganizationIsTaxExempt,
            listing.OrganizationIsNysRegistered,
            listing.SocialLinks.Select(x => new SpaceListingSocialLinkDto(x.Id, x.Platform, x.Url)).ToList(),
            listing.Hours.Select(x => new SpaceListingHourDto(x.Id, x.DayOfWeek, x.OpenTime, x.CloseTime, x.IsClosed)).ToList(),
            listing.Attachments.Select(x => new SpaceListingAttachmentDto(x.Id, x.Url, x.Type, x.FileName, x.ContentType, x.SizeBytes)).ToList(),
            listing.SpaceItemId,
            listing.SpaceEntityType,
            listing.SpaceSlug,
            listing.SpacePublishedAt,
            listing.AssignedReviewerUserId,
            listing.ModerationNote,
            listing.ReviewedAt,
            listing.CreatedAt,
            listing.UpdatedAt
        );
    }
}
