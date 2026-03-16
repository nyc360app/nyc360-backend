using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Events;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Events.Commands.ManageTickets;

public class ManageTicketTiersCommandHandler(
    IEventRepository eventRepository, 
    IUnitOfWork unitOfWork) 
    : IRequestHandler<ManageTicketTiersCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(ManageTicketTiersCommand request, CancellationToken ct)
    {
        var ev = await eventRepository.GetEventWithTicketsAsync(request.EventId, ct);

        if (ev == null)
            return StandardResponse.Failure(new ApiError("event.notFound", "Event not found"));
        
        var staffMember = ev.Staff.FirstOrDefault(s => s.UserId == request.UserId);
        if (staffMember is not { CanManageTicketing: true })
        {
            return StandardResponse.Failure(new ApiError("auth.forbidden", "You don't have permission to manage tickets for this event"));
        }

        // 2. Sync Logic
        foreach (var dto in request.Tiers)
        {
            if (dto.Id.HasValue) 
            {
                // Update existing
                var existing = ev.Tiers.FirstOrDefault(x => x.Id == dto.Id);
                if (existing == null) 
                    continue;
                    
                existing.Name = dto.Name;
                existing.Description = dto.Description;
                existing.Price = dto.Price;
                existing.QuantityAvailable = dto.QuantityAvailable < existing.QuantitySold ? existing.QuantitySold : dto.QuantityAvailable;
            }
            else 
            {
                // Add New
                ev.Tiers.Add(new EventTicketTier
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    QuantityAvailable = dto.QuantityAvailable,
                });
            }
        }

        await unitOfWork.SaveChangesAsync(ct);
        return StandardResponse.Success();
    }
}