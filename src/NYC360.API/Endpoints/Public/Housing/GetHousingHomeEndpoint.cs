using NYC360.Application.Features.Housing.Queries.Home;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;

namespace NYC360.API.Endpoints.Public.Housing;

public class GetHousingHomeEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse<HousingHomeDto>>
{
    public override void Configure()
    {
        Get("/housing/home");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        var query = new GetHousingHomeQuery(userId.Value);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}