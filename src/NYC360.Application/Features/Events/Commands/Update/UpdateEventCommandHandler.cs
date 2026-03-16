using NYC360.Application.Contracts.Authentication;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Events;
using NYC360.Domain.Enums.Events;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Events.Commands.Update;

public class UpdateEventCommandHandler(
    IEventRepository eventRepository,
    ILocationRepository locationRepository,
    IPasswordHasher passwordHasher)
    : IRequestHandler<UpdateEventCommand, StandardResponse<bool>>
{
    public async Task<StandardResponse<bool>> Handle(UpdateEventCommand request, CancellationToken ct)
    {
        var ev = await eventRepository.GetEventWithDetailsAsync(request.Id, ct);

        if (ev == null)
            return StandardResponse<bool>.Failure(new ApiError("event.notFound", "Event not found"));

        if (ev.OwnerId != request.UserId)
            return StandardResponse<bool>.Failure(new ApiError("auth.forbidden", "You don't have permission to edit this event"));

        ev.Title = request.Title.Trim();
        ev.Description = request.Description.Trim();
        ev.Category = request.Category;
        ev.StartDateTime = request.StartDateTime;
        ev.EndDateTime = request.EndDateTime;
        ev.Visibility = request.Visibility;
        ev.UpdatedAt = DateTime.UtcNow;

        if (request.Visibility == EventVisibility.PrivatePassword && !string.IsNullOrWhiteSpace(request.AccessPassword))
            ev.AccessPasswordHash = passwordHasher.Hash(request.AccessPassword);

        if (request.Address != null)
        {
            var addressId = await locationRepository.GetOrCreateAddressIdAsync(request.Address, ct);
            if (addressId > 0) ev.AddressId = addressId;
        }

        // Tiers handling is tricky without a dedicated DB context but we can manage through navigation properties
        if (request.Tiers != null)
        {
            ev.Tiers.Clear(); // In-memory update, EF will handle the sync if configured correctly or tracked
            foreach (var tierDto in request.Tiers)
            {
                ev.Tiers.Add(new EventTicketTier
                {
                    Name = tierDto.Name,
                    Description = tierDto.Description,
                    Price = tierDto.Price,
                    QuantityAvailable = tierDto.QuantityAvailable,
                    MinPerOrder = tierDto.MinPerOrder,
                    MaxPerOrder = tierDto.MaxPerOrder,
                    SaleStart = tierDto.SaleStart,
                    SaleEnd = tierDto.SaleEnd,
                    IsActive = true
                });
            }
        }

        eventRepository.Update(ev);
        await eventRepository.SaveChangesAsync(ct);
        return StandardResponse<bool>.Success(true);
    }
}
