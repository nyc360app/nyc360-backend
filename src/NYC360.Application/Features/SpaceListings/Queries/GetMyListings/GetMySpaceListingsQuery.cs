using MediatR;
using NYC360.Domain.Dtos.SpaceListings;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.SpaceListings.Queries.GetMyListings;

public record GetMySpaceListingsQuery(int UserId, int Page, int PageSize)
    : IRequest<PagedResponse<SpaceListingListItemDto>>;
