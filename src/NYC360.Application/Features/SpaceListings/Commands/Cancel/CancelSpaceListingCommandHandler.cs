using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Enums.SpaceListings;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.SpaceListings.Commands.Cancel;

public class CancelSpaceListingCommandHandler(
    ISpaceListingRepository listingRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CancelSpaceListingCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(CancelSpaceListingCommand request, CancellationToken ct)
    {
        var listing = await listingRepository.GetByIdAsync(request.ListingId, ct);
        if (listing == null)
            return StandardResponse.Failure(new ApiError("space.listing.not_found", "Listing not found."));

        if (listing.SubmitterUserId != request.UserId)
            return StandardResponse.Failure(new ApiError("space.listing.forbidden", "You cannot cancel this listing."));

        if (listing.Status is SpaceListingStatus.PublishedToSpace or SpaceListingStatus.Claimed)
            return StandardResponse.Failure(new ApiError("space.listing.cannot_cancel", "Published listings cannot be cancelled."));

        listing.Status = SpaceListingStatus.Cancelled;
        listing.Touch();
        listingRepository.Update(listing);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}
