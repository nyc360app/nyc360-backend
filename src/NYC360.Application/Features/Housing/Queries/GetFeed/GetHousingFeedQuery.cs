using MediatR;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Housing.Queries.GetFeed;

public record GetHousingFeedQuery(
    int PageNumber = 1,
    int PageSize = 10,
    bool? IsRenting = null,
    int? MinPrice = null,
    int? MaxPrice = null,
    int? LocationId = null,
    string? Search = null
) : IRequest<PagedResponse<HousingMinimalDto>>;