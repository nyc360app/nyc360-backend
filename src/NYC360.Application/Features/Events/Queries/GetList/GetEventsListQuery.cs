using MediatR;
using NYC360.Domain.Dtos.Events;
using NYC360.Domain.Entities.Events;
using NYC360.Domain.Enums.Events;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Events.Queries.GetList;

public record GetEventsListQuery(
    int PageSize = 20,
    int PageNumber = 1,
    string? SearchTerm = null,
    EventCategory? Category = null,
    EventStatus? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    int? LocationId = null
) : IRequest<PagedResponse<EventMinimalDto>>;
