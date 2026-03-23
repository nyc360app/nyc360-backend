using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.SpaceListings;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.SpaceListings.Queries.Admin;

public class GetAdminSpaceListingsQueryHandler(ISpaceListingRepository listingRepository)
    : IRequestHandler<GetAdminSpaceListingsQuery, PagedResponse<SpaceListingListItemDto>>
{
    public async Task<PagedResponse<SpaceListingListItemDto>> Handle(GetAdminSpaceListingsQuery request, CancellationToken ct)
    {
        var items = await listingRepository.GetAdminListingsAsync(
            request.Page,
            request.PageSize,
            request.Department,
            request.EntityType,
            request.Status,
            request.Search,
            ct);

        var totalCount = await listingRepository.GetAdminListingsCountAsync(
            request.Department,
            request.EntityType,
            request.Status,
            request.Search,
            ct);

        return PagedResponse<SpaceListingListItemDto>.Create(items, request.Page, request.PageSize, totalCount);
    }
}
