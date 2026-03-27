using FastEndpoints;
using NYC360.API.Models.News;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.News;

public class GetPublishedNewsPollsEndpoint(INewsPollService newsPollService)
    : Endpoint<GetPublishedNewsPollsRequest, PagedResponse<NewsPollSummaryDto>>
{
    public override void Configure()
    {
        Get("/news/polls/published");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetPublishedNewsPollsRequest req, CancellationToken ct)
    {
        var result = await newsPollService.GetPublishedAsync(req.Page, req.PageSize, ct);
        await Send.OkAsync(result, ct);
    }
}
