using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Queries.GetMyRequests;

public record GetMyRequestsQuery(
    int UserId,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResponse<HousingRequestDto>>;