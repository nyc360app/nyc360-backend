using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Enums.Events;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Events.Commands.Delete;

public class DeleteEventCommandHandler(IEventRepository eventRepository) 
    : IRequestHandler<DeleteEventCommand, StandardResponse<bool>>
{
    public async Task<StandardResponse<bool>> Handle(DeleteEventCommand request, CancellationToken ct)
    {
        var ev = await eventRepository.GetEventWithDetailsAsync(request.Id, ct);

        if (ev == null)
            return StandardResponse<bool>.Failure(new ApiError("event.notFound", "Event not found"));

        var organizer = ev.Staff.FirstOrDefault(o => o.UserId == request.UserId);
        if (organizer == null || organizer.Role != EventRole.Organizer)
            return StandardResponse<bool>.Failure(new ApiError("auth.forbidden", "Only the main organizer can delete this event"));

        eventRepository.Remove(ev);
        await eventRepository.SaveChangesAsync(ct);
        
        return StandardResponse<bool>.Success(true);
    }
}
