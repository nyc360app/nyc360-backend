using NYC360.Application.Features.Common.Queries.GlobalSearch;
using NYC360.Domain.Dtos.Common;
using NYC360.API.Models.Common;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Common;

public class GlobalSearchEndpoint(IMediator mediator) : Endpoint<GlobalSearchRequest, StandardResponse<GlobalSearchDto>>
{
    public override void Configure()
    {
        Get("/common/global-search");
    }

    public override async Task HandleAsync(GlobalSearchRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        var query = new GlobalSearchQuery(
            userId,
            req.Term,
            req.Division,
            req.Limit);
        
        var result = await mediator.Send(query, ct);

        await Send.OkAsync(result, ct);
    }
}