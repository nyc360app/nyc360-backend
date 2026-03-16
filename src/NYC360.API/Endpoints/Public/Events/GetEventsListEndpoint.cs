using NYC360.Application.Features.Events.Queries.GetList;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

using NYC360.API.Models.Events;
using NYC360.Domain.Dtos.Events;
using NYC360.Domain.Entities.Events;

namespace NYC360.API.Endpoints.Public.Events;

public class GetEventsListEndpoint(IMediator mediator) : Endpoint<GetEventsListRequest, PagedResponse<EventMinimalDto>>
{
    public override void Configure()
    {
        Get("/events");
    }

    public override async Task HandleAsync(GetEventsListRequest req, CancellationToken ct)
    {
        var query = new GetEventsListQuery(
            req.PageSize,
            req.PageNumber,
            req.SearchTerm,
            req.Category,
            req.Status,
            req.FromDate,
            req.ToDate,
            req.LocationId
        );
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}
