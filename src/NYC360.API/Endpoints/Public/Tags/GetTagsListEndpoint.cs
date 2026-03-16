using NYC360.Application.Features.Tags.Queries.List;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Wrappers;
using NYC360.API.Models.Tags;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Tags;

public class GetTagsListEndpoint(IMediator mediator) : Endpoint<GetTagsListRequest, PagedResponse<TagDto>>
{
    public override void Configure()
    {
        Get("/tags/list");
    }

    public override async Task HandleAsync(GetTagsListRequest request, CancellationToken ct)
    {
        var query = new GetTagsPagedListQuery(
            request.SearchTerm,
            request.Type,
            request.Division,
            request.Page,
            request.PageSize
        );

        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}