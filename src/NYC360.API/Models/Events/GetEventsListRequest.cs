using NYC360.Domain.Enums.Events;

namespace NYC360.API.Models.Events;

public record GetEventsListRequest(
    int PageSize = 20,
    int PageNumber = 1,
    string? SearchTerm = null,
    EventCategory? Category = null,
    EventStatus? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    int? LocationId = null
);
