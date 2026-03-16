using NYC360.Application.Features.Housing.Queries.GetAgentListings;
using NYC360.Domain.Dtos.Housing;
using NYC360.API.Models.Housing;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Housing;

public class GetAgentListingsEndpoint(IMediator mediator) 
    : Endpoint<GetAgentListingsRequest, PagedResponse<AgentListingDto>>
{
    public override void Configure()
    {
        Get("/housing/agent/listings");
    }

    public override async Task HandleAsync(GetAgentListingsRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var query = new GetAgentListingsQuery(userId.Value, req.Page, req.PageSize);
        var result = await mediator.Send(query, ct);

        await Send.OkAsync(result, ct);
    }
}
