using NYC360.Domain.Dtos.Support;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Support.Queries.GetTicketsList;

public record GetTicketsListQuery(
    SupportTicketStatus? Status = null,
    int PageNumber = 1, 
    int PageSize = 10
) : IRequest<PagedResponse<SupportTicketDto>>;