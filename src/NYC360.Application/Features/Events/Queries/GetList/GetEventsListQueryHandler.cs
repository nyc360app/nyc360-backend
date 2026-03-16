using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Events;
using NYC360.Domain.Wrappers;
using MediatR;
using NYC360.Domain.Dtos.Events;

namespace NYC360.Application.Features.Events.Queries.GetList;

public class GetEventsListQueryHandler(IEventRepository eventRepository) 
    : IRequestHandler<GetEventsListQuery, PagedResponse<EventMinimalDto>>
{
    public async Task<PagedResponse<EventMinimalDto>> Handle(GetEventsListQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await eventRepository.GetPagedEventsAsync(
            request.SearchTerm,
            (int)(request.Category ?? 0),
            (int)(request.Status ?? 0),
            request.FromDate,
            request.ToDate,
            request.LocationId,
            request.PageNumber,
            request.PageSize,
            ct);

        var dtos = items.Select(EventMinimalDto.Map).ToList();

        return PagedResponse<EventMinimalDto>.Create(dtos, request.PageNumber, request.PageSize, totalCount); 
    }
}
