using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Features.SpaceListings.Common;
using NYC360.Domain.Dtos.SpaceListings;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.SpaceListings.Queries.GetMyListingDetails;

public class GetMySpaceListingDetailsQueryHandler(ISpaceListingRepository listingRepository)
    : IRequestHandler<GetMySpaceListingDetailsQuery, StandardResponse<SpaceListingDetailsDto>>
{
    public async Task<StandardResponse<SpaceListingDetailsDto>> Handle(GetMySpaceListingDetailsQuery request, CancellationToken ct)
    {
        var listing = await listingRepository.GetByIdWithDetailsAsync(request.ListingId, ct);
        if (listing == null)
            return StandardResponse<SpaceListingDetailsDto>.Failure(new ApiError("space.listing.not_found", "Listing not found."));

        if (listing.SubmitterUserId != request.UserId)
            return StandardResponse<SpaceListingDetailsDto>.Failure(new ApiError("space.listing.forbidden", "You cannot view this listing."));

        return StandardResponse<SpaceListingDetailsDto>.Success(SpaceListingMapper.MapDetails(listing));
    }
}
