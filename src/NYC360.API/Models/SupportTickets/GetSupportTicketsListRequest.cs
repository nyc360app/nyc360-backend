using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;

namespace NYC360.API.Models.SupportTickets;

public record GetSupportTicketsListRequest(SupportTicketStatus? Status) : PagedRequest;