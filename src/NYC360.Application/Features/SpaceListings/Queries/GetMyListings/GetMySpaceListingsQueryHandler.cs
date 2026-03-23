using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.SpaceListings;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.SpaceListings.Queries.GetMyListings;

public class GetMySpaceListingsQueryHandler(ISpaceListingRepository listingRepository)
    : IRequestHandler<GetMySpaceListingsQuery, PagedResponse<SpaceListingListItemDto>>
{
    public async Task<PagedResponse<SpaceListingListItemDto>> Handle(GetMySpaceListingsQuery request, CancellationToken ct)
    {
        var items = await listingRepository.GetMyListingsAsync(request.UserId, request.Page, request.PageSize, ct);
        var totalCount = await listingRepository.GetMyListingsCountAsync(request.UserId, ct);
        return PagedResponse<SpaceListingListItemDto>.Create(items, request.Page, request.PageSize, totalCount);
    }
}
