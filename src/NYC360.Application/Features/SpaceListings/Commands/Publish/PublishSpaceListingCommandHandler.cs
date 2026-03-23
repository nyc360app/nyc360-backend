using NYC360.Application.Contracts.Infrastructure;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Features.SpaceListings.Common;
using NYC360.Domain.Enums.SpaceListings;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.SpaceListings.Commands.Publish;

public class PublishSpaceListingCommandHandler(
    ISpaceListingRepository listingRepository,
    ISpaceIntegrationService spaceIntegrationService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<PublishSpaceListingCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(PublishSpaceListingCommand request, CancellationToken ct)
    {
        var listing = await listingRepository.GetByIdWithDetailsAsync(request.ListingId, ct);
        if (listing == null)
            return StandardResponse.Failure(new ApiError("space.listing.not_found", "Listing not found."));

        if (!string.IsNullOrWhiteSpace(listing.SpaceItemId))
            return StandardResponse.Success();

        if (listing.Status is not (SpaceListingStatus.Approved or SpaceListingStatus.PublishedToSpace or SpaceListingStatus.Claimed))
            return StandardResponse.Failure(new ApiError("space.publish.invalid_status", "Listing is not ready to publish."));

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

        if (!result.Success || string.IsNullOrWhiteSpace(result.SpaceItemId))
        {
            listing.LastPublishError = result.ErrorMessage ?? "Failed to publish to Space.";
            listingRepository.Update(listing);
            await unitOfWork.SaveChangesAsync(ct);
            return StandardResponse.Failure(new ApiError("space.publish.failed", listing.LastPublishError));
        }

        listing.SpaceItemId = result.SpaceItemId;
        listing.SpaceEntityType = result.SpaceEntityType;
        listing.SpaceSlug = result.SpaceSlug;
        listing.SpacePublishedAt = DateTime.UtcNow;
        listing.LastPublishError = null;

        listing.Status = listing.IsClaimingOwnership ? SpaceListingStatus.Claimed : SpaceListingStatus.PublishedToSpace;
        listing.Touch();
        listingRepository.Update(listing);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}
