using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Emails;
using NYC360.Application.Models.Emails;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Support.Commands.ReplyTicket;

public class ReplyTicketCommandHandler(
    ISupportTicketRepository supportTicketRepository,
    IUserRepository userRepository,
    IEmailService emailService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ReplyTicketCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(ReplyTicketCommand request, CancellationToken ct)
    {
        // 1. Get Ticket & validate
        var ticket = await supportTicketRepository.GetByIdAsync(request.TicketId, ct);
        if (ticket == null)
            return StandardResponse.Failure(new ApiError("ticket.notFound", "Ticket not found."));

        // 2. Get User Profile to get their email/name
        var userProfile = await userRepository.GetProfileByUserIdAsync(ticket.CreatorId ?? 0, ct);
        var targetEmail = ticket.Email ?? userProfile?.Email;
        var targetName = ticket.Name ?? userProfile?.GetFullName() ?? "User";

        if (string.IsNullOrEmpty(targetEmail))
            return StandardResponse.Failure(new ApiError("ticket.noEmail", "Could not find a recipient email."));

        // 3. Update Ticket State
        ticket.Status = SupportTicketStatus.Active; // Or 'Resolved' if you add that status
        ticket.AssignedAdminId = request.AdminUserId;
        ticket.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(ct);

        // 4. Send the Reply Email via Support account
        var emailModel = new TicketReplyEmailModel(
            targetName, 
            ticket.Subject, 
            request.ReplyMessage, 
            ticket.Id);

        await emailService.SendTicketReplyAsync(targetEmail, emailModel, ct);

        return StandardResponse.Success();
    }
}