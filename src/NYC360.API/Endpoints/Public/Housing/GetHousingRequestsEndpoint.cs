using NYC360.Application.Features.Housing.Queries.GetRequests;
using NYC360.Domain.Dtos.Housing;
using NYC360.API.Models.Housing;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Housing;

public class GetHousingRequestsEndpoint(IMediator mediator) 
    : Endpoint<GetAgentRequestsRequest, PagedResponse<HousingRequestDto>>
{
    public override void Configure()
    {
        Get("/housing/agent/requests");
    }

    public override async Task HandleAsync(GetAgentRequestsRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        var query = new GetAgentRequestsQuery(userId.Value, req.Page, req.PageSize);
        var result = await mediator.Send(query, ct);

        await Send.OkAsync(result, ct);
    }
}