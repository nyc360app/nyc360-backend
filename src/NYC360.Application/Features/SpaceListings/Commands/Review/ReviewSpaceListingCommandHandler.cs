using NYC360.Application.Contracts.Infrastructure;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Features.SpaceListings.Common;
using NYC360.Domain.Entities.SpaceListings;
using NYC360.Domain.Enums.SpaceListings;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.SpaceListings.Commands.Review;

public class ReviewSpaceListingCommandHandler(
    ISpaceListingRepository listingRepository,
    ILocationRepository locationRepository,
    ISpaceIntegrationService spaceIntegrationService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ReviewSpaceListingCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(ReviewSpaceListingCommand request, CancellationToken ct)
    {
        var listing = await listingRepository.GetByIdWithDetailsAsync(request.ListingId, ct);
        if (listing == null)
            return StandardResponse.Failure(new ApiError("space.listing.not_found", "Listing not found."));

        if (request.Decision is not (SpaceListingStatus.Approved or SpaceListingStatus.Rejected or SpaceListingStatus.NeedsChanges))
            return StandardResponse.Failure(new ApiError("space.review.invalid_status", "Invalid review decision."));

        var fromStatus = listing.Status;
        listing.Status = request.Decision;
        listing.ModerationNote = string.IsNullOrWhiteSpace(request.ModerationNote) ? null : request.ModerationNote.Trim();
        listing.ReviewedAt = DateTime.UtcNow;
        listing.ReviewedByUserId = request.ReviewerUserId;
        listing.Touch();

        listing.ReviewEntries.Add(new SpaceListingReviewEntry
        {
            ReviewerUserId = request.ReviewerUserId,
            FromStatus = fromStatus,
            ToStatus = listing.Status,
            Note = listing.ModerationNote
        });

        if (request.Decision == SpaceListingStatus.Rejected)
        {
            listing.OwnershipStatus = listing.IsClaimingOwnership ? SpaceListingOwnershipStatus.Rejected : SpaceListingOwnershipStatus.None;
            listing.ClaimedByUserId = null;
        }

        if (request.Decision == SpaceListingStatus.Approved)
        {
            await SpaceListingLocationSync.EnsureLocationLinkedAsync(listing, locationRepository, ct);
            listing.OwnershipStatus = listing.IsClaimingOwnership ? SpaceListingOwnershipStatus.Approved : SpaceListingOwnershipStatus.None;
            if (listing.IsClaimingOwnership)
                listing.ClaimedByUserId = listing.SubmitterUserId;
            var publishResult = await PublishToSpaceAsync(listing, spaceIntegrationService, ct);
            if (!publishResult.IsSuccess)
            {
                listingRepository.Update(listing);
                await unitOfWork.SaveChangesAsync(ct);
                return publishResult;
            }
        }

        listingRepository.Update(listing);
        await unitOfWork.SaveChangesAsync(ct);
        return StandardResponse.Success();
    }

    private static async Task<StandardResponse> PublishToSpaceAsync(
        SpaceListing listing,
        ISpaceIntegrationService spaceIntegrationService,
        CancellationToken ct)
    {
        if (!string.IsNullOrWhiteSpace(listing.SpaceItemId))
            return StandardResponse.Success();

        var addressLine = SpaceListingNormalizer.BuildAddressLine(listing.Street, listing.BuildingNumber);
        var result = await spaceIntegrationService.CreateListingAsync(
            new SpaceIntegrationRequest(
                $"space-listing-{listing.Id}",
                listing.EntityType,
                listing.Name,
                listing.Description,
                listing.Website,
                listing.PhoneNumber,
                listing.PublicEmail,
                listing.ContactName,
                addressLine,
                listing.Borough,
                listing.Neighborhood,
                listing.ZipCode,
                listing.Tags),
            ct);

        listing.LastPublishAttemptAt = DateTime.UtcNow;

        if (result.IsPublishSkipped)
        {
            listing.LastPublishError = null;
            listing.Status = listing.IsClaimingOwnership ? SpaceListingStatus.Claimed : SpaceListingStatus.PublishedToSpace;
            return StandardResponse.Success();
        }

        if (!result.Success || string.IsNullOrWhiteSpace(result.SpaceItemId))
        {
            listing.LastPublishError = result.ErrorMessage ?? "Failed to publish to Space.";
            return StandardResponse.Failure(new ApiError("space.publish.failed", listing.LastPublishError));
        }

        listing.SpaceItemId = result.SpaceItemId;
        listing.SpaceEntityType = result.SpaceEntityType;
        listing.SpaceSlug = result.SpaceSlug;
        listing.SpacePublishedAt = DateTime.UtcNow;
        listing.LastPublishError = null;
        listing.Status = listing.IsClaimingOwnership ? SpaceListingStatus.Claimed : SpaceListingStatus.PublishedToSpace;
        return StandardResponse.Success();
    }
}
