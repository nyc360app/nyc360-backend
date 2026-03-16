using NYC360.Application.Features.Divisions.Common;
using NYC360.Domain.Dtos.Pages;
using NYC360.API.Models.Homes;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Homes;

public class GetCommonHomeEndpoint(IMediator mediator) 
    : Endpoint<GetCommonHomeRequest, StandardResponse<DivisionHomeDto>>
{
    public override void Configure()
    {
        Get("/feeds/common/home");
    }

    public override async Task HandleAsync(GetCommonHomeRequest req, CancellationToken ct)
    {
        var userId = User.GetId();

        var query = new GetCommonHomeQuery(
            req.Division, 
            userId, 
            req.Limit
        );

        var result = await mediator.Send(query, ct);
        
        await Send.OkAsync(result, ct);
    }
}