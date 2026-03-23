using MediatR;
using NYC360.Domain.Dtos.SpaceListings;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.SpaceListings;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.SpaceListings.Queries.Admin;

public record GetAdminSpaceListingsQuery(
    int Page,
    int PageSize,
    Category? Department,
    SpaceListingEntityType? EntityType,
    SpaceListingStatus? Status,
    string? Search)
    : IRequest<PagedResponse<SpaceListingListItemDto>>;
