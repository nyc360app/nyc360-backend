using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Events;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums.Events;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Events.Queries.GetById;

public class GetEventByIdQueryHandler(IEventRepository eventRepository) 
    : IRequestHandler<GetEventByIdQuery, StandardResponse<EventDto>>
{
    public async Task<StandardResponse<EventDto>> Handle(GetEventByIdQuery request, CancellationToken ct)
    {
        var ev = await eventRepository.GetEventWithDetailsAsync(request.Id, ct);

        if (ev == null)
        {
            return StandardResponse<EventDto>.Failure(new ApiError("event.notFound", "Event not found"));
        }

        var dto = EventDto.Map(ev);

        return StandardResponse<EventDto>.Success(dto);
    }
}
