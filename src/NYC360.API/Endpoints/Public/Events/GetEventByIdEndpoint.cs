using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Events.Queries.GetById;
using NYC360.Domain.Dtos.Events;
using NYC360.Domain.Wrappers;

using NYC360.API.Models.Events;

namespace NYC360.API.Endpoints.Public.Events;

public class GetEventByIdEndpoint(IMediator mediator) : Endpoint<GetEventByIdRequest, StandardResponse<EventDto>>
{
    public override void Configure()
    {
        Get("/events/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetEventByIdRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new GetEventByIdQuery(req.Id), ct);
        await Send.OkAsync(result, ct);
    }
}
