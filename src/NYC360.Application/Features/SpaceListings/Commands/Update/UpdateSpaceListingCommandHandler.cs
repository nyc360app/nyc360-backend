using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using NYC360.Application.Features.SpaceListings.Common;
using NYC360.Domain.Entities.SpaceListings;
using NYC360.Domain.Enums.SpaceListings;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.SpaceListings.Commands.Update;

public class UpdateSpaceListingCommandHandler(
    ISpaceListingRepository listingRepository,
    ILocationRepository locationRepository,
    IUnitOfWork unitOfWork,
    ILocalStorageService storageService)
    : IRequestHandler<UpdateSpaceListingCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateSpaceListingCommand request, CancellationToken ct)
    {
        var listing = await listingRepository.GetByIdWithDetailsAsync(request.ListingId, ct);
        if (listing == null)
            return StandardResponse.Failure(new ApiError("space.listing.not_found", "Listing not found."));

        if (listing.SubmitterUserId != request.UserId)
            return StandardResponse.Failure(new ApiError("space.listing.forbidden", "You cannot edit this listing."));

        if (listing.Status is not (SpaceListingStatus.Draft or SpaceListingStatus.NeedsChanges or SpaceListingStatus.Rejected))
            return StandardResponse.Failure(new ApiError("space.listing.locked", "Listing cannot be edited in its current status."));

        var existingOwnershipDocs = listing.Attachments.Any(x => x.Type == SpaceListingAttachmentType.OwnershipDocument);
        if (request.IsClaimingOwnership && !existingOwnershipDocs && (request.OwnershipDocuments == null || request.OwnershipDocuments.Count == 0))
            return StandardResponse.Failure(new ApiError("space.ownership.docs.required", "Ownership documents are required to claim ownership."));

        if (request.Address.LocationId.HasValue)
        {
            var exists = await locationRepository.ExistsAsync(request.Address.LocationId.Value, ct);
            if (!exists)
                return StandardResponse.Failure(new ApiError("space.location.invalid", "LocationId does not exist."));
        }

        var normalizedName = SpaceListingNormalizer.NormalizeName(request.Name);
        var normalizedWebsite = SpaceListingNormalizer.NormalizeWebsite(request.Website);
        var normalizedEmail = SpaceListingNormalizer.NormalizeEmail(request.PublicEmail);
        var normalizedPhone = SpaceListingNormalizer.NormalizePhone(request.PhoneNumber);

        listing.Department = request.Department;
        listing.EntityType = request.EntityType;
        listing.Name = request.Name.Trim();
        listing.NameNormalized = normalizedName;
        listing.Description = request.Description.Trim();
        listing.LocationId = request.Address.LocationId;
        listing.LocationName = request.LocationName?.Trim();
        listing.Borough = request.Borough?.Trim();
        listing.Neighborhood = request.Neighborhood?.Trim();
        listing.Street = request.Address.Street?.Trim();
        listing.BuildingNumber = request.Address.BuildingNumber?.Trim();
        listing.ZipCode = request.Address.ZipCode?.Trim();
        listing.Website = request.Website?.Trim();
        listing.WebsiteNormalized = normalizedWebsite;
        listing.PhoneNumber = request.PhoneNumber?.Trim();
        listing.PhoneNumberNormalized = normalizedPhone;
        listing.PublicEmail = request.PublicEmail?.Trim();
        listing.PublicEmailNormalized = normalizedEmail;
        listing.ContactName = request.ContactName?.Trim();
        listing.SubmitterNote = request.SubmitterNote?.Trim();
        listing.IsClaimingOwnership = request.IsClaimingOwnership;
        listing.OwnershipStatus = request.IsClaimingOwnership ? SpaceListingOwnershipStatus.Requested : SpaceListingOwnershipStatus.None;
        if (!request.IsClaimingOwnership)
            listing.ClaimedByUserId = null;
        listing.Categories = SpaceListingDepartmentPolicy.BuildCategories(request.Department, request.Categories);
        listing.Tags = request.Tags ?? [];
        listing.BusinessIndustry = request.BusinessIndustry;
        listing.BusinessSize = request.BusinessSize;
        listing.BusinessServiceArea = request.BusinessServiceArea;
        listing.BusinessServices = request.BusinessServices;
        listing.BusinessOwnershipType = request.BusinessOwnershipType;
        listing.BusinessIsLicensedInNyc = request.BusinessIsLicensedInNyc;
        listing.BusinessIsInsured = request.BusinessIsInsured;
        listing.OrganizationType = request.OrganizationType;
        listing.OrganizationFundType = request.OrganizationFundType;
        listing.OrganizationServices = request.OrganizationServices;
        listing.OrganizationIsTaxExempt = request.OrganizationIsTaxExempt;
        listing.OrganizationIsNysRegistered = request.OrganizationIsNysRegistered;

        listing.SocialLinks.Clear();
        foreach (var link in request.SocialLinks)
        {
            listing.SocialLinks.Add(new SpaceListingSocialLink
            {
                Platform = link.Platform,
                Url = link.Url.Trim()
            });
        }

        listing.Hours.Clear();
        foreach (var hour in request.Hours)
        {
            listing.Hours.Add(new SpaceListingHour
            {
                DayOfWeek = hour.DayOfWeek,
                OpenTime = hour.OpenTime,
                CloseTime = hour.CloseTime,
                IsClosed = hour.IsClosed
            });
        }

        await AddAttachmentsAsync(listing, request.Images, SpaceListingAttachmentType.Image, ct);
        await AddAttachmentsAsync(listing, request.Documents, SpaceListingAttachmentType.Document, ct);
        await AddAttachmentsAsync(listing, request.OwnershipDocuments, SpaceListingAttachmentType.OwnershipDocument, ct);
        await AddAttachmentsAsync(listing, request.ProofDocuments, SpaceListingAttachmentType.ProofDocument, ct);

        listing.Touch();
        listingRepository.Update(listing);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }

    private async Task AddAttachmentsAsync(
        SpaceListing listing,
        List<Microsoft.AspNetCore.Http.IFormFile>? files,
        SpaceListingAttachmentType type,
        CancellationToken ct)
    {
        if (files == null || files.Count == 0)
            return;

        foreach (var file in files)
        {
            if (!IsAllowedFile(file, type))
                continue;

            var fileName = await storageService.SaveFileAsync(file, "space-listings", ct);
            listing.Attachments.Add(new SpaceListingAttachment
            {
                Url = "@local://" + fileName,
                Type = type,
                FileName = file.FileName,
                ContentType = file.ContentType,
                SizeBytes = file.Length
            });
        }
    }

    private static bool IsAllowedFile(Microsoft.AspNetCore.Http.IFormFile file, SpaceListingAttachmentType type)
    {
        var contentType = file.ContentType.ToLowerInvariant();
        if (type == SpaceListingAttachmentType.Image)
            return contentType is "image/jpeg" or "image/png" or "image/webp";

        return contentType is "application/pdf";
    }
}
