using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Enums.Events;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Events.Commands.Publish;

public class PublishEventCommandHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<PublishEventCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(PublishEventCommand request, CancellationToken ct)
    {
        var evt = await eventRepository.GetByIdAsync(request.EventId, ct);
        if (evt == null)
            return StandardResponse.Failure(new ApiError("event.notFound", "Event not found"));

        if (evt.OwnerId != request.UserId)
            return StandardResponse.Failure(new ApiError("auth.forbidden", "You don't have permission to publish this event"));
        
        if(evt.Status == EventStatus.Published)
            return StandardResponse.Failure(new ApiError("auth.alreadypublished", "This event is already published"));
        
        evt.Status = EventStatus.Published;
        await unitOfWork.SaveChangesAsync(ct);
        
        return StandardResponse.Success();
    }
}