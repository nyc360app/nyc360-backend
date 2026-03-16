using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Emails;
using NYC360.Application.Models.Emails;
using NYC360.Domain.Entities.Support;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Support.Commands.CreateTicket;

public class CreateTicketCommandHandler(
    ISupportTicketRepository supportTicketRepository,
    IUserRepository userRepository,
    IEmailService emailService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateTicketCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(CreateTicketCommand request, CancellationToken ct)
    {
        // 1. Fetch User details for the email templates
        ApplicationUser? user = null;
        if (request.UserId != null)
        {
            user = await userRepository.GetUserWithProfileByIdAsync(request.UserId.Value, ct);
            if (user == null)
            {
                return StandardResponse.Failure(new ApiError("user.notFound", "User profile not found."));
            }
        }

        // 2. Persist the Ticket
        var ticket = new SupportTicket
        {
            CreatorId = request.UserId,
            Email = request.Email,
            Name = request.Name,
            Subject = request.Subject,
            Description = request.Message
        };

        // Assuming your UnitOfWork or a specific SupportRepository handles this
        await supportTicketRepository.AddAsync(ticket, ct);
        await unitOfWork.SaveChangesAsync(ct);

        // 3. Prepare Email Data
        var userFullName = user != null ? user.GetFullName() : request.Name;
        var userEmail = user != null ? user.Email : request.Email;

        if (string.IsNullOrWhiteSpace(userEmail))
        {
            // Log this internally if possible, then return failure
            return StandardResponse.Failure(new ApiError("ticket.emailMissing", "A valid email address is required to process this ticket."));
        }

        // 4. Send Confirmation to User (From Support Account)
        var userEmailModel = new SupportTicketModel(
            ticket.Id,
            userFullName!, 
            userEmail!, 
            request.Subject, 
            request.Message);
            
        await emailService.SendSupportTicketAsync(userEmailModel, ct);

        return StandardResponse.Success();
    }
}