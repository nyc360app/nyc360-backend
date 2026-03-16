using MediatR;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Housing.Queries.GetAdminList;

public record GetAdminHousingListQuery(
    int PageNumber = 1,
    int PageSize = 10,
    bool? IsPublished = null,
    string? Search = null
) : IRequest<PagedResponse<AgentListingDto>>;
