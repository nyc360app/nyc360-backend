using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Enums.SpaceListings;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.SpaceListings.Commands.Resubmit;

public class ResubmitSpaceListingCommandHandler(
    ISpaceListingRepository listingRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ResubmitSpaceListingCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(ResubmitSpaceListingCommand request, CancellationToken ct)
    {
        var listing = await listingRepository.GetByIdAsync(request.ListingId, ct);
        if (listing == null)
            return StandardResponse.Failure(new ApiError("space.listing.not_found", "Listing not found."));

        if (listing.SubmitterUserId != request.UserId)
            return StandardResponse.Failure(new ApiError("space.listing.forbidden", "You cannot resubmit this listing."));

        if (listing.Status is not (SpaceListingStatus.NeedsChanges or SpaceListingStatus.Rejected or SpaceListingStatus.Draft))
            return StandardResponse.Failure(new ApiError("space.listing.resubmit_not_allowed", "Listing cannot be resubmitted in its current status."));

        listing.Status = SpaceListingStatus.Pending;
        if (listing.IsClaimingOwnership)
            listing.OwnershipStatus = SpaceListingOwnershipStatus.Requested;
        listing.Touch();
        listingRepository.Update(listing);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}
