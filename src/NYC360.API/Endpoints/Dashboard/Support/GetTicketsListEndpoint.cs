using NYC360.Application.Features.Support.Queries.GetTicketsList;
using NYC360.API.Models.SupportTickets;
using NYC360.Domain.Dtos.Support;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Dashboard.Support;

public class GetTicketsListEndpoint(IMediator mediator) 
    : Endpoint<GetSupportTicketsListRequest, PagedResponse<SupportTicketDto>>
{
    public override void Configure()
    {
        Get("/support-dashboard/list");
        Permissions(Domain.Constants.Permissions.SupportTickets.View);
    }

    public override async Task HandleAsync(GetSupportTicketsListRequest req, CancellationToken ct)
    {
        var query = new GetTicketsListQuery(req.Status, req.Page, req.PageSize);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}