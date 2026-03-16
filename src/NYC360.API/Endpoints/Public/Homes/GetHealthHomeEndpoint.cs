using NYC360.Application.Features.Health.Queries.GetFeed;
using NYC360.Domain.Dtos.Pages;
using NYC360.API.Models.Homes;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Homes;

public class GetHealthHomeEndpoint(IMediator mediator) 
    : Endpoint<GetHealthHomeRequest, StandardResponse<HealthFeedDto>>
{
    public override void Configure()
    {
        Get("/feeds/health/home");
    }

    public override async Task HandleAsync(GetHealthHomeRequest request, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var query = new GetHealthFeedQuery(userId.Value, request.LocationId);
        var result = await mediator.Send(query, ct);
        
        await Send.OkAsync(result, ct);
    }
}