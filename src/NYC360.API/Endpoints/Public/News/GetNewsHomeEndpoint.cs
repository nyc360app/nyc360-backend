using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.News;
using NYC360.Application.Features.Divisions.Common;
using NYC360.Domain.Dtos.Pages;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.News;

public class GetNewsHomeEndpoint(IMediator mediator)
    : Endpoint<GetNewsHomeRequest, StandardResponse<DivisionHomeDto>>
{
    public override void Configure()
    {
        Get("/news/home");
    }

    public override async Task HandleAsync(GetNewsHomeRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        var result = await mediator.Send(new GetCommonHomeQuery(Category.News, userId, req.Limit), ct);
        await Send.OkAsync(result, ct);
    }
}
