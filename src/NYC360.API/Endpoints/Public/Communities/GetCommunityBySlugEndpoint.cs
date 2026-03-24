using NYC360.Application.Features.Communities.Queries.GetBySlug;
using NYC360.API.Models.Communities;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Dtos.Communities;

namespace NYC360.API.Endpoints.Public.Communities;

public class GetCommunityBySlugEndpoint(IMediator mediator) : Endpoint<GetCommunityBySlugRequest, StandardResponse<CommunityHomePageDto>>
{
    public override void Configure()
    {
        Get("/communities/{Slug}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetCommunityBySlugRequest req, CancellationToken ct)
    {
        var query = new GetCommunityBySlugQuery(
            User.GetId(),
            req.Slug,
            req.Page,
            req.PageSize
        );
        var result = await mediator.Send(query, ct);

        await Send.OkAsync(result, ct);
    }
}
