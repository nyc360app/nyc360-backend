using NYC360.Application.Features.Housing.Queries.GetById;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Housing;

public class GetHousingByIdEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse<HousingDetailsDto>>
{
    public override void Configure()
    {
        Get("/housing/{id}");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        var id = Route<int>("id");
        var query = new GetHousingByIdQuery(userId, id);
        var result = await mediator.Send(query, ct);
        
        await Send.OkAsync(result, ct);
    }
}