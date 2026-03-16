using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Support;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Support.Queries.GetTicketsList;

public class GetTicketsListHandler(ISupportTicketRepository supportTicketRepository) 
    : IRequestHandler<GetTicketsListQuery, PagedResponse<SupportTicketDto>>
{
    public async Task<PagedResponse<SupportTicketDto>> Handle(GetTicketsListQuery request, CancellationToken ct)
    {
        var (list, total) = await supportTicketRepository.GetPagedAsync(request.Status, request.PageNumber, request.PageSize, ct);
        return PagedResponse<SupportTicketDto>.Create(list, request.PageNumber, request.PageSize, total);
    }
}