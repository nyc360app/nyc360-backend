using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Support.Commands.CloseTicket;

public class CloseTicketCommandHandler(
    ISupportTicketRepository supportTicketRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CloseTicketCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(CloseTicketCommand request, CancellationToken ct)
    {
        var ticket = await supportTicketRepository.GetByIdAsync(request.TicketId, ct);
        
        if (ticket == null)
            return StandardResponse.Failure(new ApiError("ticket.notFound", "Ticket not found."));

        ticket.Status = SupportTicketStatus.Closed;
        ticket.AssignedAdminId = request.AdminUserId;
        ticket.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}