using MediatR;
using NYC360.Domain.Dtos.SpaceListings;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.SpaceListings.Queries.Admin;

public record GetAdminSpaceListingDetailsQuery(int ListingId)
    : IRequest<StandardResponse<SpaceListingDetailsDto>>;
