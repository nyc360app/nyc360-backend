using NYC360.Application.Features.Housing.Queries.GetMyRequests;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Housing;

public class GetMyRequestsEndpoint (IMediator mediator) : EndpointWithoutRequest<PagedResponse<HousingRequestDto>>
{
    public override void Configure()
    {
        Get("/housing/requests/my-requests");
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        var query = new GetMyRequestsQuery(userId.Value);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}