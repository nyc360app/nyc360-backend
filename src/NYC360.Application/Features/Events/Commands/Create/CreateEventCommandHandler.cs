using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Entities.Events;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Events.Commands.Create;

public class CreateEventCommandHandler(
    IUserRepository userRepository,
    IEventRepository eventRepository,
    ILocationRepository locationRepository,
    IUnitOfWork unitOfWork,
    ILocalStorageService localStorageService)
    : IRequestHandler<CreateEventCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(CreateEventCommand request, CancellationToken ct)
    {
        var user = await userRepository.GetByIdWithStatsAsync(request.UserId, ct);

        if (user == null)
            return StandardResponse<int>.Failure(new ApiError("user.notFound", "User not found"));

        var ev = new Event
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Category = request.Category,
            Type = request.Type,

            StartDateTime = request.StartDateTime,
            EndDateTime = request.EndDateTime,
            
            OwnerId = request.UserId
        };
        
        if (request.Address != null)
        {
            var addressId = await locationRepository.GetOrCreateAddressIdAsync(request.Address, ct);
            if (addressId > 0)
            {
                ev.AddressId = addressId;
            }
        }

        ev.Staff.Add(new EventStaff
        {
            UserId = request.UserId,
            Role = request.UserRole,
            HasFinancialAuthority = true,
            CanManageTicketing = true,
            CanManageCampaigns = true,
            CanViewRevenueAttribution = true,
            HasApprovalAuthority = true,
            CanEnforceCompliance = true,
            HasFullAnalyticsAccess = true
        });

        if (request.Attachments != null && request.Attachments.Count != 0)
        {
            foreach (var attachment in request.Attachments)
            {
                var url = await localStorageService.SaveFileAsync(attachment, "events", ct);
                ev.Attachments.Add(new EventAttachment { Url = url });
            }
        }

        await eventRepository.AddAsync(ev, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<int>.Success(ev.Id);
    }
}