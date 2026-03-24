using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using NYC360.Application.Features.SpaceListings.Common;
using NYC360.Domain.Entities.SpaceListings;
using NYC360.Domain.Enums.SpaceListings;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.SpaceListings.Commands.Submit;

public class SubmitSpaceListingCommandHandler(
    ISpaceListingRepository listingRepository,
    ILocationRepository locationRepository,
    IUnitOfWork unitOfWork,
    ILocalStorageService storageService)
    : IRequestHandler<SubmitSpaceListingCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(SubmitSpaceListingCommand request, CancellationToken ct)
    {
        if (request.Address == null)
            return StandardResponse<int>.Failure(new ApiError("space.address.required", "Address details are required."));

        var normalizedName = SpaceListingNormalizer.NormalizeName(request.Name);
        var normalizedWebsite = SpaceListingNormalizer.NormalizeWebsite(request.Website);
        var normalizedEmail = SpaceListingNormalizer.NormalizeEmail(request.PublicEmail);
        var normalizedPhone = SpaceListingNormalizer.NormalizePhone(request.PhoneNumber);

        if (request.Address.LocationId.HasValue)
        {
            var exists = await locationRepository.ExistsAsync(request.Address.LocationId.Value, ct);
            if (!exists)
                return StandardResponse<int>.Failure(new ApiError("space.location.invalid", "LocationId does not exist."));
        }
        else
        {
            if (string.IsNullOrWhiteSpace(request.LocationName) &&
                string.IsNullOrWhiteSpace(request.Borough) &&
                string.IsNullOrWhiteSpace(request.Neighborhood))
            {
                return StandardResponse<int>.Failure(new ApiError("space.location.missing", "Location details are required when no LocationId is provided."));
            }
        }

        var duplicate = await listingRepository.FindDuplicateAsync(
            normalizedName,
            request.Address.LocationId,
            request.Address.ZipCode,
            normalizedWebsite,
            normalizedPhone,
            normalizedEmail,
            ct);

        if (duplicate != null && duplicate.Status is not SpaceListingStatus.Rejected and not SpaceListingStatus.Cancelled)
        {
            if (duplicate.OwnershipStatus == SpaceListingOwnershipStatus.Approved)
                return StandardResponse<int>.Failure(new ApiError("space.listing.ownership_conflict", "This listing is already claimed."));

            return StandardResponse<int>.Failure(new ApiError("space.listing.duplicate", "A similar listing already exists."));
        }

        if (request.IsClaimingOwnership && (request.OwnershipDocuments == null || request.OwnershipDocuments.Count == 0))
            return StandardResponse<int>.Failure(new ApiError("space.ownership.docs.required", "Ownership documents are required to claim ownership."));

        var recentCount = await listingRepository.CountRecentSubmissionsAsync(request.UserId, DateTime.UtcNow.AddHours(-24), ct);
        if (recentCount >= 10)
            return StandardResponse<int>.Failure(new ApiError("space.listing.rate_limit", "Too many submissions in the last 24 hours."));

        var entity = new SpaceListing
        {
            SubmitterUserId = request.UserId,
            Department = request.Department,
            EntityType = request.EntityType,
            Status = request.SaveAsDraft ? SpaceListingStatus.Draft : SpaceListingStatus.Pending,
            OwnershipStatus = request.IsClaimingOwnership ? SpaceListingOwnershipStatus.Requested : SpaceListingOwnershipStatus.None,
            Name = request.Name.Trim(),
            NameNormalized = normalizedName,
            Description = request.Description.Trim(),
            LocationId = request.Address.LocationId,
            LocationName = request.LocationName?.Trim(),
            Borough = request.Borough?.Trim(),
            Neighborhood = request.Neighborhood?.Trim(),
            Street = request.Address.Street?.Trim(),
            BuildingNumber = request.Address.BuildingNumber?.Trim(),
            ZipCode = request.Address.ZipCode?.Trim(),
            Website = request.Website?.Trim(),
            WebsiteNormalized = normalizedWebsite,
            PhoneNumber = request.PhoneNumber?.Trim(),
            PhoneNumberNormalized = normalizedPhone,
            PublicEmail = request.PublicEmail?.Trim(),
            PublicEmailNormalized = normalizedEmail,
            ContactName = request.ContactName?.Trim(),
            SubmitterNote = request.SubmitterNote?.Trim(),
            IsClaimingOwnership = request.IsClaimingOwnership,
            Categories = SpaceListingDepartmentPolicy.BuildCategories(request.Department, request.Categories),
            Tags = request.Tags ?? [],
            BusinessIndustry = request.BusinessIndustry,
            BusinessSize = request.BusinessSize,
            BusinessServiceArea = request.BusinessServiceArea,
            BusinessServices = request.BusinessServices,
            BusinessOwnershipType = request.BusinessOwnershipType,
            BusinessIsLicensedInNyc = request.BusinessIsLicensedInNyc,
            BusinessIsInsured = request.BusinessIsInsured,
            OrganizationType = request.OrganizationType,
            OrganizationFundType = request.OrganizationFundType,
            OrganizationServices = request.OrganizationServices,
            OrganizationIsTaxExempt = request.OrganizationIsTaxExempt,
            OrganizationIsNysRegistered = request.OrganizationIsNysRegistered
        };

        foreach (var link in request.SocialLinks)
        {
            entity.SocialLinks.Add(new SpaceListingSocialLink
            {
                Platform = link.Platform,
                Url = link.Url.Trim()
            });
        }

        foreach (var hour in request.Hours)
        {
            entity.Hours.Add(new SpaceListingHour
            {
                DayOfWeek = hour.DayOfWeek,
                OpenTime = hour.OpenTime,
                CloseTime = hour.CloseTime,
                IsClosed = hour.IsClosed
            });
        }

        await AddAttachmentsAsync(entity, request.Images, SpaceListingAttachmentType.Image, ct);
        await AddAttachmentsAsync(entity, request.Documents, SpaceListingAttachmentType.Document, ct);
        await AddAttachmentsAsync(entity, request.OwnershipDocuments, SpaceListingAttachmentType.OwnershipDocument, ct);
        await AddAttachmentsAsync(entity, request.ProofDocuments, SpaceListingAttachmentType.ProofDocument, ct);

        await listingRepository.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<int>.Success(entity.Id);
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
