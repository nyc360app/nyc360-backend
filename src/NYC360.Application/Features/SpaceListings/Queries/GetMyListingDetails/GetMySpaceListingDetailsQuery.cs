using MediatR;
using NYC360.Domain.Dtos.SpaceListings;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.SpaceListings.Queries.GetMyListingDetails;

public record GetMySpaceListingDetailsQuery(int ListingId, int UserId)
    : IRequest<StandardResponse<SpaceListingDetailsDto>>;
