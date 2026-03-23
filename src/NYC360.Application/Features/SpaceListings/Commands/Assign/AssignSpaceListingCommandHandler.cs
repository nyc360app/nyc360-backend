using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Enums.SpaceListings;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.SpaceListings.Commands.Assign;

public class AssignSpaceListingCommandHandler(
    ISpaceListingRepository listingRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AssignSpaceListingCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(AssignSpaceListingCommand request, CancellationToken ct)
    {
        var listing = await listingRepository.GetByIdAsync(request.ListingId, ct);
        if (listing == null)
            return StandardResponse.Failure(new ApiError("space.listing.not_found", "Listing not found."));

        listing.AssignedReviewerUserId = request.ReviewerUserId;
        if (listing.Status == SpaceListingStatus.Pending)
            listing.Status = SpaceListingStatus.UnderReview;

        listing.Touch();
        listingRepository.Update(listing);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}
